using Foundatio.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HRM.Common;
using HRM.Common.RedisCache;
using HRM.Common.Resource;
using HRM.Models.Cores.Function;
using HRM.Models.Cores.GroupUser;
using HRM.Models.Cores.Menu;
using HRM.Models.Entities;
using HRM.Services.Cores.GroupUsers;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;

namespace HRM.Services.Cores.Menu
{
    public class MenuSystemService : IMenuSystemService
    {
        private readonly ICacheClient _cacheClient;
        private readonly CoreProjectContext _sqlContext;
        private readonly RedisCacheSettingModel _redisCacheSettings;
        private readonly IGroupUserService groupUser;
        public MenuSystemService(CoreProjectContext sqlContext, ICacheClient cacheClient, IOptions<RedisCacheSettingModel> options, IGroupUserService groupUserService)
        {
            this._sqlContext = sqlContext;
            _cacheClient = cacheClient;
            _redisCacheSettings = options.Value;
            groupUser = groupUserService;
        }

        /// <summary>
        /// Tìm kiếm menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<MenuViewModel>> SearchMenuAsync(MenuSearchModel searchModel)
        {
            SearchBaseResultModel<MenuViewModel> searchResult = new SearchBaseResultModel<MenuViewModel>();
            var dataQuery = (from a in _sqlContext.MenuSystem.AsNoTracking()
                             orderby a.Index
                             select new MenuViewModel
                             {
                                 Id = a.Id,
                                 //TitleKeyTranslate = a.TitleKeyTranslate,
                                 TitleDefault = a.TitleDefault,
                                 Icon = a.Icon,
                                 Url = a.Url,
                                 ParentId = a.ParentId,
                                 IsDisable = a.IsDisable,
                                 Index = a.Index,
                                 IsDefaultMenu = a.IsDefaultMenu,
                                 //FunctionAuto = a.FunctionAuto
                             }).OrderBy(e => e.Index).Distinct().AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.TitleDefault))
            {
                dataQuery = dataQuery.Where(u => u.TitleDefault.ToUpper().Contains(searchModel.TitleDefault.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.TitleKeyTranslate))
            {
                dataQuery = dataQuery.Where(u => u.TitleKeyTranslate.ToUpper().Contains(searchModel.TitleKeyTranslate.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Url))
            {
                dataQuery = dataQuery.Where(u => u.Url.ToUpper().Contains(searchModel.Url.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Icon))
            {
                dataQuery = dataQuery.Where(u => u.Icon.ToUpper().Contains(searchModel.Icon.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }
            var nemus = dataQuery.ToList();
            var _pareHRM = nemus.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            foreach (var menu in _pareHRM)
            {
                menu.Children = GetSubMenu(menu, nemus).OrderBy(e => e.Index).ToList();
                if (menu.Children.Count > 0)
                {
                    menu.Type = "collapsable";
                }
                else
                {
                    menu.Type = "item";
                }
            }
            var result = _pareHRM.OrderBy(e => e.Index).ToList();
            searchResult.TotalItems = result.Count();
            var listResult = result.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách menu cho left bar
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        public async Task<List<MenuViewModel>> GetMenuAsync(string userId)
        {
            List<MenuViewModel> result = new List<MenuViewModel>(); // Get from Cachimng
            string key = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{userId}";

            CacheValue<List<MenuViewModel>> cachedValue = await _cacheClient.GetAsync<List<MenuViewModel>>(key);
            if (cachedValue.HasValue)
            {
                result = cachedValue.Value;
            }

            if (result == null || result.Count == 0)
            {
                var menus = (from a in _sqlContext.MenuSystem.AsNoTracking()
                             orderby a.Index
                             select new MenuViewModel
                             {
                                 Id = a.Id,
                                 TitleDefault = a.TitleDefault,
                                 //TitleKeyTranslate = string.IsNullOrEmpty(a.TitleKeyTranslate) ? a.TitleDefault : a.TitleKeyTranslate,
                                 Icon = a.Icon,
                                 Url = a.Url,
                                 ParentId = a.ParentId,
                                 IsDisable = a.IsDisable,
                                 Index = a.Index,
                                 IsDefaultMenu = a.IsDefaultMenu
                             }).OrderBy(e => e.Index).Distinct().ToList();

                //Danh sách menu được phân cho người dùng
                List<string> menuPermissions;

                //Nếu không phải tài khoản root thì chỉ lấy các menu hiện
                if (!userId.Equals(HRMConstants.IdUserRootFix))
                {
                    menus = menus.Where(s => !s.IsDisable).ToList();
                    menuPermissions = (from a in _sqlContext.MenuSystemPermission.AsNoTracking()
                                       join b in _sqlContext.UserPermission.Where(e => userId.Equals(e.UserId)).AsNoTracking() on a.FunctionId equals b.FunctionId
                                       select a.MenuSystemId).Distinct().ToList();
                }
                else
                {
                    menuPermissions = menus.Select(s => s.Id).ToList();
                }

                var stringMenuPermissions = string.Join(";", menuPermissions);

                var _pareHRM = menus.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                foreach (var menu in _pareHRM)
                {
                    if (stringMenuPermissions.Contains(menu.Id) || menu.IsDefaultMenu)
                    {
                        menu.Match = 1;
                    }
                    menu.Children = GetSubMenu(menu, menus, stringMenuPermissions).OrderBy(e => e.Index).ToList();
                    if (menu.Children.Count > 0)
                    {
                        menu.Type = "collapsable";
                    }
                    else
                    {
                        menu.Type = "item";
                    }
                }

                return _pareHRM.Where(o => o.Match > 0).OrderBy(e => e.Index).ToList();

                await _cacheClient.RemoveAsync(key);
                await _cacheClient.AddAsync<List<MenuViewModel>>(key, result, new TimeSpan(1, 0, 0, 0));
            }

            return result;
        }

        /// <summary>
        /// lấy con cho nemu chính
        /// </summary>
        /// <returns></returns>
        private List<MenuViewModel> GetSubMenu(MenuViewModel parentMenu, List<MenuViewModel> menus, string stringMenuPermissions = null)
        {
            var children = menus.Where(e => e.ParentId == parentMenu.Id).OrderBy(o => o.Index).ToList();

            foreach (var item in children)
            {
                if ((!string.IsNullOrEmpty(stringMenuPermissions) && stringMenuPermissions.Contains(item.Id)) || item.IsDefaultMenu)
                {
                    item.Match = 1;
                }
                item.Children = GetSubMenu(item, menus, stringMenuPermissions);
                if (item.Children.Count > 0)
                {
                    item.Type = "collapsable";
                }
                else
                {
                    item.Type = "item";
                }
                parentMenu.Match = parentMenu.Match + item.Match;
            }
            if (!string.IsNullOrEmpty(stringMenuPermissions))
            {
                children = children.Where(e => e.Match > 0).ToList();
            }
            return children;
        }

        /// <summary>
        /// Xem chi tiết menu theo id
        /// </summary>
        /// <returns></returns>
        public async Task<MenuViewModel> GetMenuByIdAsync(string id)
        {
            MenuViewModel result = new MenuViewModel();
            result = (from a in _sqlContext.MenuSystem.AsNoTracking()
                      where id.Equals(a.Id)
                      select new MenuViewModel
                      {
                          Id = a.Id,
                          //TitleKeyTranslate = a.TitleKeyTranslate,
                          TitleDefault = a.TitleDefault,
                          Icon = a.Icon,
                          Url = a.Url,
                          ParentId = a.ParentId,
                          IsDisable = a.IsDisable,
                          Index = a.Index,
                          IsDefaultMenu = a.IsDefaultMenu,
                          //FunctionAuto = a.FunctionAuto,
                          //SystemFunctionConfigId = a.SystemFunctionConfigId
                      }).FirstOrDefault();

            result.ListPermission = (from a in _sqlContext.MenuSystemPermission.AsNoTracking().Where(w => w.MenuSystemId.Equals(id))
                                     join b in _sqlContext.Function.AsNoTracking() on a.FunctionId equals b.Id
                                     select new PermissionModel()
                                     {
                                         Id = a.FunctionId,
                                         Code = b.Code,
                                         Name = b.Name,
                                         Index = b.Index,
                                         IsChecked = true
                                     }).OrderBy(o => o.Index).ToList();
            return result;
        }

        /// <summary>
        /// Thêm menu
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        public async Task CreateMenuAsync(CreateMenuModel model)
        {
            if (string.IsNullOrEmpty(model.TitleDefault))
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0020);
            }
            var checkurl = _sqlContext.MenuSystem.FirstOrDefault(e => !string.IsNullOrEmpty(model.Url) && e.Url.Equals(model.Url));
            if (checkurl != null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0025);
            }

            //Nếu là chức năng phát triển và list quyền >0 thì kiểm tra mã quyền đã tồn tại chưa
            if (!model.FunctionAuto && model.ListPermission?.Count > 0)
            {
                string listPermissionCode = string.Join(";", model.ListPermission.Select(s => s.Code).ToList());
                var functionsExit = _sqlContext.Function.Where(s => listPermissionCode.Contains(s.Code)).ToList();
                if (functionsExit.Count > 0)
                {
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0033);
                }
            }

            //Thêm menu
            MenuSystem menu = new MenuSystem()
            {
                Id = Guid.NewGuid().ToString(),
                TitleDefault = model.TitleDefault,
                //TitleKeyTranslate = model.TitleDefault,
                Icon = model.Icon,
                Url = model.Url,
                ParentId = model.ParentId,
                IsDisable = model.IsDisable,
                IsDefaultMenu = model.IsDefaultMenu,
                //FunctionAuto = model.FunctionAuto,
                //SystemFunctionConfigId = model.SystemFunctionConfigId
            };
            _sqlContext.MenuSystem.Add(menu);

            #region Thêm vào bảng con
            if (model.ListPermission?.Count > 0)
            {
                //thêm vào bảng menusytempermission
                MenuSystemPermission menusystempermission;
                Function function;
                UserPermission userpermission;
                UserGroupFunction usergroupfunction;
                int index = 0;
                foreach (var item in model.ListPermission)
                {
                    //Add quyền mới
                    function = new Function()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = item.Code,
                        Name = item.Name,
                        Index = index,
                    };
                    _sqlContext.Function.Add(function);
                    index++;

                    //Add quyền mới cho menu
                    menusystempermission = new MenuSystemPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MenuSystemId = menu.Id,
                        FunctionId = function.Id
                    };
                    _sqlContext.MenuSystemPermission.Add(menusystempermission);

                    //phân quyền luôn cho admin
                    userpermission = new UserPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FunctionId = function.Id,
                        UserId = HRMConstants.IdUserAdminFix
                    };
                    _sqlContext.UserPermission.Add(userpermission);

                    //phân quyền luôn cho root
                    userpermission = new UserPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FunctionId = function.Id,
                        UserId = HRMConstants.IdUserRootFix
                    };
                    _sqlContext.UserPermission.Add(userpermission);

                    //Thêm quyền cho nhóm quyền admin
                    usergroupfunction = new UserGroupFunction()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FunctionId = function.Id,
                        UserGroupId = HRMConstants.GroupAdminId
                    };
                    _sqlContext.UserGroupFunction.Add(usergroupfunction);
                }
            }
            #endregion
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Sửa menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <returns></returns>
        public async Task UpdateMenuAsync(string id, CreateMenuModel model)
        {
            var menu = _sqlContext.MenuSystem.FirstOrDefault(e => e.Id.Equals(id));
            if (menu == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0024);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    //Cập nhật thông tin menu
                    menu.TitleDefault = model.TitleDefault;
                    //menu.TitleKeyTranslate = model.TitleKeyTranslate;
                    menu.Url = model.Url;
                    menu.ParentId = model.ParentId;
                    menu.Icon = model.Icon;
                    menu.IsDisable = model.IsDisable;
                    menu.IsDefaultMenu = model.IsDefaultMenu;
                    //menu.FunctionAuto = model.FunctionAuto;
                    //menu.SystemFunctionConfigId = model.SystemFunctionConfigId;
                    _sqlContext.MenuSystem.Update(menu);

                    List<MenuSystemPermission> listMenuPermissionRemove = null;
                    if (model.ListPermission?.Count > 0)
                    {
                        var listFuntionId = string.Join(";", model.ListPermission.Select(s => s.Id).ToList());
                        //Xóa các quyền nếu có
                        listMenuPermissionRemove = _sqlContext.MenuSystemPermission.Where(r => r.MenuSystemId.Equals(id) && !listFuntionId.Contains(r.FunctionId)).ToList();
                    }
                    else
                    {
                        //Xóa các quyền nếu có
                        listMenuPermissionRemove = _sqlContext.MenuSystemPermission.Where(r => r.MenuSystemId.Equals(id)).ToList();
                    }

                    if (listMenuPermissionRemove?.Count > 0)
                    {
                        _sqlContext.MenuSystemPermission.RemoveRange(listMenuPermissionRemove);

                        var listFuntionIdRemove = string.Join(";", listMenuPermissionRemove.Select(s => s.Id).ToList());
                        //Xóa trong nhóm người dùng
                        var userGroupFunctionRemove = _sqlContext.UserGroupFunction.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                        if (userGroupFunctionRemove.Count > 0)
                            _sqlContext.UserGroupFunction.RemoveRange(userGroupFunctionRemove);

                        //Xóa quyền trong người dùng
                        var userPermissionRemove = _sqlContext.UserPermission.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                        if (userPermissionRemove.Count > 0)
                            _sqlContext.UserPermission.RemoveRange(userPermissionRemove);

                        //Xóa quyền trong bảng quyền
                        var functionRemove = _sqlContext.Function.Where(s => listFuntionIdRemove.Contains(s.Id)).ToList();
                        if (functionRemove.Count > 0)
                            _sqlContext.Function.RemoveRange(functionRemove);
                    }

                    #region Thêm quyền cho menu và các bảng liên kết
                    if (model.ListPermission.Count > 0)
                    {
                        //thêm vào bảng menusytempermission
                        MenuSystemPermission menusystempermission;
                        Function function;
                        UserPermission userpermission;
                        UserGroupFunction usergroupfunction;
                        int index = 0;
                        foreach (var item in model.ListPermission)
                        {
                            //Thêm mới hoặc cập nhật quyền
                            function = _sqlContext.Function.FirstOrDefault(s => s.Id.Equals(item.Id)) ?? new Function();
                            function.Code = item.Code;
                            function.Name = item.Name;
                            function.Index = index;
                            if (string.IsNullOrEmpty(function.Id))
                            {
                                function.Id = Guid.NewGuid().ToString();
                                _sqlContext.Function.Add(function);
                            }
                            else
                            {
                                _sqlContext.Function.Update(function);
                            }
                            index++;

                            //Add quyền mới cho menu
                            menusystempermission = _sqlContext.MenuSystemPermission.FirstOrDefault(s => s.FunctionId.Equals(item.Id)) ?? new MenuSystemPermission();
                            menusystempermission.MenuSystemId = menu.Id;
                            menusystempermission.FunctionId = function.Id;
                            if (string.IsNullOrEmpty(menusystempermission.Id))
                            {
                                menusystempermission.Id = Guid.NewGuid().ToString();
                                _sqlContext.MenuSystemPermission.Add(menusystempermission);
                            }
                            else
                            {
                                _sqlContext.MenuSystemPermission.Update(menusystempermission);
                            }

                            //phân quyền luôn cho admin
                            userpermission = _sqlContext.UserPermission.FirstOrDefault(s => s.FunctionId.Equals(item.Id) && s.UserId.Equals(HRMConstants.IdUserAdminFix)) ?? new UserPermission();
                            userpermission.FunctionId = function.Id;
                            userpermission.UserId = HRMConstants.IdUserAdminFix;
                            if (string.IsNullOrEmpty(userpermission.Id))
                            {
                                userpermission.Id = Guid.NewGuid().ToString();
                                _sqlContext.UserPermission.Add(userpermission);
                            }
                            else
                            {
                                _sqlContext.UserPermission.Update(userpermission);
                            }

                            //phân quyền luôn cho root
                            userpermission = _sqlContext.UserPermission.FirstOrDefault(s => s.FunctionId.Equals(item.Id) && s.UserId.Equals(HRMConstants.IdUserRootFix)) ?? new UserPermission();
                            userpermission.FunctionId = function.Id;
                            userpermission.UserId = HRMConstants.IdUserRootFix;
                            if (string.IsNullOrEmpty(userpermission.Id))
                            {
                                userpermission.Id = Guid.NewGuid().ToString();
                                _sqlContext.UserPermission.Add(userpermission);
                            }
                            else
                            {
                                _sqlContext.UserPermission.Update(userpermission);
                            }

                            //Thêm quyền cho nhóm quyền admin
                            usergroupfunction = _sqlContext.UserGroupFunction.FirstOrDefault(s => s.FunctionId.Equals(item.Id) && s.UserGroupId.Equals(HRMConstants.GroupAdminId)) ?? new UserGroupFunction();
                            usergroupfunction.FunctionId = function.Id;
                            usergroupfunction.UserGroupId = HRMConstants.GroupAdminId;
                            if (string.IsNullOrEmpty(usergroupfunction.Id))
                            {
                                usergroupfunction.Id = Guid.NewGuid().ToString();
                                _sqlContext.UserGroupFunction.Add(usergroupfunction);
                            }
                            else
                            {
                                _sqlContext.UserGroupFunction.Update(usergroupfunction);
                            }
                        }
                    }
                    #endregion

                    _sqlContext.SaveChanges();
                    trans.Commit();
                    RemoveRedis(id);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Ẩn/Hiện menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <param name="userId">Id người thao tác</param>
        /// <returns></returns>
        public async Task DisbaleMenuAsync(string id, string userId)
        {
            var menu = _sqlContext.MenuSystem.FirstOrDefault(e => e.Id.Equals(id));
            if (menu == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0024);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    menu.IsDisable = !menu.IsDisable;
                    _sqlContext.MenuSystem.Update(menu);
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Xóa menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <param name="userId">Id người thao tác</param>
        /// <returns></returns>
        public async Task DeleteMenuByIdAsync(string id, string userId)
        {
            var menu = _sqlContext.MenuSystem.FirstOrDefault(e => e.Id.Equals(id));
            if (menu == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0024);
            }

            var listMenuPermissionRemove = _sqlContext.MenuSystemPermission.Where(r => r.MenuSystemId.Equals(id)).ToList();
            if (listMenuPermissionRemove?.Count > 0)
            {
                _sqlContext.MenuSystemPermission.RemoveRange(listMenuPermissionRemove);

                var listFuntionIdRemove = string.Join(";", listMenuPermissionRemove.Select(s => s.Id).ToList());
                //Xóa trong nhóm người dùng
                var userGroupFunctionRemove = _sqlContext.UserGroupFunction.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                if (userGroupFunctionRemove.Count > 0)
                    _sqlContext.UserGroupFunction.RemoveRange(userGroupFunctionRemove);

                //Xóa quyền trong người dùng
                var userPermissionRemove = _sqlContext.UserPermission.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                if (userPermissionRemove.Count > 0)
                    _sqlContext.UserPermission.RemoveRange(userPermissionRemove);

                //Xóa quyền trong bảng quyền
                var functionRemove = _sqlContext.Function.Where(s => listFuntionIdRemove.Contains(s.Id)).ToList();
                if (functionRemove.Count > 0)
                    _sqlContext.Function.RemoveRange(functionRemove);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.MenuSystem.Remove(menu);
                    await _sqlContext.SaveChangesAsync();
                    trans.Commit();

                    //Xóa cache user login liên quan đến menu này
                    RemoveRedis(id);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa rediscache theo khi xóa hoặc disable menu
        /// </summary>
        /// <param name="menuid">Id menu</param>
        public void RemoveRedis(string menuid)
        {
            var users = (from a in _sqlContext.UserPermission.AsNoTracking()
                         join b in _sqlContext.MenuSystemPermission.AsNoTracking() on a.FunctionId equals b.FunctionId
                         join c in _sqlContext.MenuSystem.Where(e => menuid.Equals(e.Id)).AsNoTracking() on b.MenuSystemId equals c.Id
                         select a.UserId).Distinct().ToList();
            // Key lưu cache login
            foreach (var item in users)
            {
                string keymenu = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{item}";
                if (_cacheClient.ExistsAsync(keymenu).Result)
                {
                    var IsCheck = _cacheClient.RemoveAsync(keymenu).Result;
                }
            }

        }

        /// <summary>
        /// Cập nhật vị trí hiển thị của menu
        /// </summary>
        /// <param name="listMenu">Danh sách menu</param>
        /// <returns></returns>
        public async Task UpdateIndexMenu(List<UpdateIndexMenuModel> listMenu)
        {
            if (listMenu != null && listMenu.Count > 0)
            {
                var listMenuUpdate = _sqlContext.MenuSystem.ToList();
                int index = 0;
                MenuSystem itemMenuUdate;
                foreach (var item in listMenu)
                {
                    itemMenuUdate = listMenuUpdate.FirstOrDefault(x => x.Id == item.Id);
                    if (itemMenuUdate != null)
                    {
                        itemMenuUdate.ParentId = "";
                        itemMenuUdate.Index = index;
                        index++;
                    }

                    if (item.Children != null && item.Children.Count > 0)
                        this.UpdateIndexSubMenu(item.Id, listMenuUpdate, item.Children);
                }
                using (var trans = _sqlContext.Database.BeginTransaction())
                {
                    try
                    {
                        _sqlContext.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _sqlContext.ChangeTracker.Clear();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Cập nhật vị trí hiển thị của sub menu
        /// </summary>
        /// <param name="parentId">Id menu cha</param>
        /// <param name="listMenuUpdate">Danh sách menu</param>
        /// <param name="subMenu">Danh sách menu con</param>
        private void UpdateIndexSubMenu(string parentId, List<MenuSystem> listMenuUpdate, List<UpdateIndexMenuModel> subMenu)
        {
            if (subMenu != null && subMenu.Count > 0)
            {
                int index = 0;
                MenuSystem itemMenuUdate;
                foreach (var item in subMenu)
                {
                    itemMenuUdate = listMenuUpdate.FirstOrDefault(x => x.Id == item.Id);
                    if (itemMenuUdate != null)
                    {
                        itemMenuUdate.ParentId = parentId;
                        itemMenuUdate.Index = index;
                        index++;
                    }

                    if (item.Children != null && item.Children.Count > 0)
                        this.UpdateIndexSubMenu(item.Id, listMenuUpdate, item.Children);
                }
            }
        }

        #region Code xử lý lấy các quyền Fix của chức năng tự động
        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        /// <returns></returns>
        public async Task<List<FunctionAutoModel>> GetListFuntionAuto()
        {
            //List<FunctionAutoModel> listFunctionAuto = _sqlContext.FunctionAuto.Select(s => new FunctionAutoModel()
            //{
            //    Id = s.Id,
            //    Code = s.Code,
            //    Name = s.Name,
            //    Index = s.Index
            //}).OrderBy(o => o.Index).ToList();

            //return listFunctionAuto;
            return null;
        }
        #endregion
    }
}
