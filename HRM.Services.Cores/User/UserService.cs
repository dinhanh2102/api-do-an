using Microsoft.EntityFrameworkCore;
using HRM.Common;
using HRM.Common.Resource;
using HRM.Common.Utils;
using HRM.Models.Cores.Function;
using HRM.Models.Cores.GroupFunction;
using HRM.Models.Cores.User;
using HRM.Models.Entities;
using HRM.Services.Cores.Auth;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;

namespace HRM.Services.Cores.Users
{
    public class UserService : IUserService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly IAuthService _authService;
        public UserService(CoreProjectContext sqlContext, IAuthService authService)
        {
            this._sqlContext = sqlContext;
            this._authService = authService;
        }

        /// <summary>
        /// Tìm kiếm tài khoản
        /// </summary>
        /// <param name="searchModel">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<UserSearchResultModel>> SearchUserAsync(UserSearchModel searchModel)
        {
            SearchBaseResultModel<UserSearchResultModel> searchResult = new SearchBaseResultModel<UserSearchResultModel>();

            var dataQuery = (from a in _sqlContext.User.AsNoTracking()
                             orderby a.UserName
                             where !a.Id.Equals(HRMConstants.IdUserRootFix)
                             select new
                             {
                                 a.Id,
                                 a.UserName,
                                 a.FullName,
                                 a.LockoutEnabled,
                                 a.Description,
                             }).AsQueryable();

            //Lọc dữ liệu theo UserId
            if (!string.IsNullOrEmpty(searchModel.UserId))
            {
                var checkRole = _sqlContext.User.AsNoTracking().FirstOrDefault(a => a.Id.Equals(searchModel.UserId)).UserGroupId;
                if (checkRole == "NV")
                    dataQuery = dataQuery.Where(a => a.Id.ToUpper().Contains(searchModel.UserId.ToUpper()));
            }
            //Loch dữ liệu theo tên
            if (!string.IsNullOrEmpty(searchModel.UserName))
            {
                dataQuery = dataQuery.Where(a => a.UserName.ToUpper().Contains(searchModel.UserName.ToUpper()));
            }
            //Lọc dữ liệu theo tình trạng
            if (searchModel.LockoutEnabled.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.LockoutEnabled == searchModel.LockoutEnabled);
            }
            //Loch dữ liệu theo họ tên
            if (!string.IsNullOrEmpty(searchModel.FullName))
            {
                dataQuery = dataQuery.Where(a => a.FullName.ToUpper().Contains(searchModel.FullName.ToUpper()));
            }
            //Loch dữ liệu theo mô tả
            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                dataQuery = dataQuery.Where(a => a.Description.ToUpper().Contains(searchModel.Description.ToUpper()));
            }

            //Sắp xếp dữ liệu theo trường thông tin truyền xuống
            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);
            }

            //Tổng số bản ghi
            searchResult.TotalItems = dataQuery.Count();
            //Lấy danh sách theo phân trang
            searchResult.DataResults = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList()
                .Select(s => new UserSearchResultModel()
                {
                    Id = s.Id,
                    UserName = s.UserName,
                    FullName = s.FullName,
                    LockoutEnabled = s.LockoutEnabled,
                    Description = s.Description,
                }).ToList();

            return searchResult;
        }

        /// <summary>
        /// Thêm mới tài khoản
        /// </summary>
        /// <param name="model">Modle thông tin tài khoản</param>
        /// <param name="userId">Id người thêm mới</param>
        /// <returns></returns>
        public async Task CreateUserAsync(UserCreateModel model, string userId)
        {
            //Xử lý khoảng trống
            model.UserName = model.UserName?.Trim();
            model.Email = model.Email?.Trim();
            //model.PhoneNumber = model.PhoneNumber?.Trim();

            //Kiểm tra tồn tại tài khoản
            var userName = _sqlContext.User.AsNoTracking().FirstOrDefault(a => a.UserName.Equals(model.UserName));
            if (userName != null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0006);
            }

            //Kiểm tra tồn tại mail
            var email = _sqlContext.User.AsNoTracking().FirstOrDefault(a => a.Email.Equals(model.Email) && !string.IsNullOrEmpty(model.Email));
            if (email != null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0007);
            }

            //Kiểm tra tồn tại số điện thoại
            //var phoneNumber = _sqlContext.User.AsNoTracking().FirstOrDefault(a => a.PhoneNumber.Equals(model.PhoneNumber) && !string.IsNullOrEmpty(model.PhoneNumber));
            //if (phoneNumber != null)
            //{
            //    throw HRMException.CreateInstance(MessageResourceKey.MSG0008);
            //}

            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Avatar = model.Avatar,
                Email = model.Email,
                //PhoneNumber = model.PhoneNumber,
                SecurityStamp = PasswordUtils.CreateSecurityStamp(),
                LockoutEnabled = model.LockoutEnabled,
                UserGroupId = model.UserGroupId,
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
                FullName = model.FullName,
                Description = model.Description,
                
            };
            user.PasswordHash = PasswordUtils.ComputeHash($"{model.Password}{user.SecurityStamp}");
            _sqlContext.User.Add(user);

            // Thêm mới bảng quyền
            List<UserPermission> userPermissions = new List<UserPermission>();
            UserPermission userPermission;
            foreach (var item in model.ListGroupFunction)
            {
                if (item.Permissions.Count > 0)
                {
                    var userpermission = AddPermission(item.Permissions, user.Id);
                    userPermissions.AddRange(userpermission);

                }
                if (item.Children.Count > 0)
                {
                    var listper = FindPermission(item);
                    var userpermission = AddPermission(listper, user.Id);
                    userPermissions.AddRange(userpermission);
                }
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.UserPermission.AddRange(userPermissions);
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
        /// Tìm quyền
        /// </summary>
        /// <param name="group">Thông tin nhóm quyền</param>
        /// <returns></returns>
        private List<FunctionModel> FindPermission(GroupFunctionModel group)
        {
            List<FunctionModel> userPermissions = new List<FunctionModel>();
            var children = group.Children;
            foreach (var item in children)
            {
                userPermissions.AddRange(item.Permissions);
                if (item.Children.Count > 0)
                {
                    var listpermissionchild = FindPermission(item);
                    userPermissions.AddRange(listpermissionchild);
                }
            }

            return userPermissions;
        }

        /// <summary>
        /// Add quyền cho tài khoản
        /// </summary>
        /// <param name="permissions">Danh sách quyền</param>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        private List<UserPermission> AddPermission(List<FunctionModel> permissions, string userId)
        {
            List<UserPermission> userPermissions = new List<UserPermission>();
            UserPermission userPermission;
            foreach (var per in permissions)
            {
                if (per.IsChecked)
                {
                    userPermission = new UserPermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        FunctionId = per.Id,
                    };

                    userPermissions.Add(userPermission);
                }
            }
            return userPermissions;
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="model">Modle thông tin cập nhật</param>
        /// <param name="userId">Id người cập nhật</param>
        /// <returns></returns>
        public async Task UpdateUserAsync(string id, UserCreateModel model, string userId = null)
        {
            //Xử lý khoảng trống
            model.Email = model.Email?.Trim();
            //model.PhoneNumber = model.PhoneNumber.Trim();

            var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(id));
            if (user == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }
            //Kiểm tra tồn tại mail
            var email = _sqlContext.User.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(id) && a.Email.Equals(model.Email) && !string.IsNullOrEmpty(model.Email));
            if (email != null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0007);
            }

            //Kiểm tra tồn tại số điện thoại
            //var phoneNumber = _sqlContext.User.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(id) && a.PhoneNumber.Equals(model.PhoneNumber) && !string.IsNullOrEmpty(model.PhoneNumber));
            //if (phoneNumber != null)
            //{
            //    throw HRMException.CreateInstance(MessageResourceKey.MSG0008);
            //}
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                user.Avatar = model.Avatar;
                user.Email = model.Email;
                //user.PhoneNumber = model.PhoneNumber;
                user.LockoutEnabled = model.LockoutEnabled;
                user.UserGroupId = model.UserGroupId;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;
                user.Description = model.Description;
                user.FullName = model.FullName;

                var userPer = _sqlContext.UserPermission.Where(r => r.UserId.Equals(id)).ToList();
                string perOld = string.Join(";", userPer.Select(s => s.FunctionId).OrderBy(o => o));
                if (userPer != null)
                {
                    _sqlContext.UserPermission.RemoveRange(userPer);
                }
                List<UserPermission> userPermissions = new List<UserPermission>();
                UserPermission userPermission;

                foreach (var item in model.ListGroupFunction)
                {
                    if (item.Permissions.Count > 0)
                    {
                        var userpermission = AddPermission(item.Permissions, user.Id);
                        userPermissions.AddRange(userpermission);

                    }
                    if (item.Children.Count > 0)
                    {
                        var listper = FindPermission(item);
                        var userpermission = AddPermission(listper, user.Id);
                        userPermissions.AddRange(userpermission);
                    }
                }
                var _pernew = userPermissions.Select(e => e.FunctionId).ToList();
                _sqlContext.UserPermission.AddRange(userPermissions);
                string perNew = string.Join(";", _pernew);
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                    //nếu thay đổi nhóm tài khoản hoặc thay đổi quyền trong nhóm tài khoản thì xóa cache của tài khoản
                    if (!perOld.Equals(perNew))
                    {
                        // Key lưu cache login
                        _authService?.RemoveRedis(id);
                    }
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
        /// Mở/Khóa tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="isunlock">Trạng thái</param>
        /// <returns></returns>
        public async Task UserLockAsync(string id, bool isunlock)
        {
            var user = _sqlContext.User.FirstOrDefault(r => r.Id.Equals(id));

            if (user == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }
            if (user != null)
            {
                user.LockoutEnabled = isunlock;
                _authService.RemoveRedis(id);
                _sqlContext.SaveChanges();
            }
        }

        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        public async Task<UserCreateModel> GetUserByIdAsnyc(string userId)
        {
            // Lấy thông tin user
            var result = (from a in _sqlContext.User.AsNoTracking()
                          where a.Id.Equals(userId)
                          join b in _sqlContext.UserGroup.AsNoTracking()
                          on a.UserGroupId equals b.Id into bg
                          from _b in bg.DefaultIfEmpty()
                          select new UserCreateModel()
                          {
                              Id = a.Id,
                              UserName = a.UserName,
                              Avatar = a.Avatar,
                              Email = a.Email,
                              FullName = a.FullName,
                              //PhoneNumber = a.PhoneNumber,
                              LockoutEnabled = a.LockoutEnabled,
                              UserGroupId = a.UserGroupId,
                              NameGroupUser = _b.Name,
                              Description = a.Description
                          }).FirstOrDefault();

            if (result == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }
            result.ListGroupFunction = GetPermissionAsync(result.UserGroupId, userId).Result;

            return result;
        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="userid">Id người xóa</param>
        /// <returns></returns>
        public async Task DeleteUserAsync(string id, string userid)
        {
            var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(id));
            var userPermission = _sqlContext.UserPermission.Where(a => a.UserId.Equals(id)).ToList();
            //var userHistory = _sqlContext.UserHistory.Where(a => a.UserId.Equals(id)).ToList();
            var refreshToken = _sqlContext.RefreshToken.Where(a => a.UserId.Equals(id)).ToList();

            if (user == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                if (userPermission.Count > 0)
                {
                    _sqlContext.UserPermission.RemoveRange(userPermission);
                }
                //if (userHistory.Count > 0)
                //{
                //    _sqlContext.UserHistory.RemoveRange(userHistory);
                //}
                if (refreshToken.Count > 0)
                {
                    _sqlContext.RefreshToken.RemoveRange(refreshToken);
                }

                if (user != null)
                {
                    _sqlContext.User.Remove(user);
                    _authService?.RemoveRedis(id);
                }
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
        /// Lấy quyền danh sách quyền
        /// </summary>
        /// <param name="groupUserId">Id nhóm quyền</param>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        public async Task<List<GroupFunctionModel>> GetPermissionAsync(string groupUserId, string userId = null)
        {
            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();
            //Danh sách menu
            var menuSystems = (from a in _sqlContext.MenuSystem.AsNoTracking()
                               where !a.IsDefaultMenu
                               select new GroupFunctionModel
                               {
                                   Id = a.Id,
                                   Name = a.TitleDefault,
                                   ParentId = a.ParentId,
                                   Index = a.Index
                               }).OrderBy(o => o.Index).ToList();

            List<FunctionModel> permissions = new List<FunctionModel>();
            var _permissions = (from p in _sqlContext.Function.AsNoTracking()
                                join b in _sqlContext.MenuSystemPermission.AsNoTracking() on p.Id equals b.FunctionId
                                orderby p.Code
                                select new FunctionModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Code = p.Code,
                                    IsChecked = true,
                                    MenuSystemId = b.MenuSystemId,
                                }).ToList();
            List<string> fucnction = new List<string>();
            if (!string.IsNullOrEmpty(groupUserId))
            {
                fucnction = (from a in _sqlContext.UserGroupFunction.AsNoTracking()
                             where groupUserId.Equals(a.UserGroupId)
                             select a.FunctionId
                          ).ToList();
            }
            else
            {
                fucnction = (from a in _sqlContext.UserGroupFunction.AsNoTracking()
                             select a.FunctionId
                          ).ToList();
            }


            var string_functionid = string.Join(";", fucnction);
            _permissions = _permissions.Where(e => string_functionid.Contains(e.Id)).ToList();

            if (!string.IsNullOrEmpty(userId))
            {
                var permissionsold = (from a in _sqlContext.UserPermission.AsNoTracking()
                                      where userId.Equals(a.UserId)
                                      select a.FunctionId).ToList();
                var permissionid_string = string.Join(";", permissionsold);
                foreach (var permission in _permissions)
                {
                    if (permissionid_string.Contains(permission.Id))
                    {
                        permission.IsChecked = true;
                    }
                    else
                    {
                        permission.IsChecked = false;
                    }
                    permissions.Add(permission);
                }
            }
            else
            {
                permissions = _permissions;
            }
            var list_GFunctionid = string.Join(";", permissions.Select(e => e.MenuSystemId).Distinct().ToList());

            var pareHRM = menuSystems.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            foreach (var ite in pareHRM)
            {
                ite.Permissions = permissions.Where(t => t.MenuSystemId.Equals(ite.Id)).ToList();
                ite.PermissionTotal = ite.Permissions.Count;
                ite.IsChecked = ite.Permissions.Count(r => !r.IsChecked) == 0;
                ite.CheckCount = ite.Permissions.Count(r => r.IsChecked);
                ite.Children = GetChildenPermission(ite, menuSystems, permissions);
                if (ite.Permissions.Count == 0)
                {
                    ite.CountPermission = "";
                }
                else
                {
                    ite.CountPermission = ite.CheckCount.ToString() + "/" + ite.PermissionTotal.ToString();
                }

                groupFunctions.Add(ite);
            }
            groupFunctions = groupFunctions.Where(e => e.Permissions.Count > 0 || e.Children.Count > 0).ToList();
            return groupFunctions;
        }

        /// <summary>
        /// Gét nhóm quyền con
        /// </summary>
        /// <param name="group">Nhóm quyền</param>
        /// <param name="listgroup">Danh sách nhóm quyền</param>
        /// <param name="permissions">Dánh sách quyền</param>
        /// <returns></returns>
        private List<GroupFunctionModel> GetChildenPermission(GroupFunctionModel group, List<GroupFunctionModel> listgroup, List<FunctionModel> permissions)
        {
            List<GroupFunctionModel> result = new List<GroupFunctionModel>();
            var children = listgroup.Where(e => group.Id.Equals(e.ParentId)).ToList();
            foreach (var child in children)
            {
                child.Permissions = permissions.Where(t => t.MenuSystemId.Equals(child.Id)).ToList();
                child.PermissionTotal = child.Permissions.Count;
                child.IsChecked = child.Permissions.Count(r => !r.IsChecked) == 0;
                child.CheckCount = child.Permissions.Count(r => r.IsChecked);
                child.Children = GetChildenPermission(child, listgroup, permissions);
                if (child.Permissions.Count == 0)
                {
                    child.CountPermission = "";
                }
                else
                {
                    child.CountPermission = child.CheckCount.ToString() + "/" + child.PermissionTotal.ToString();
                }
                result.Add(child);
            }
            return result.Where(e => e.Permissions.Count > 0 || e.Children.Count > 0).ToList();
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="model">Model thông tin mậy khẩu tahy đổi</param>
        /// <returns></returns>
        public async Task ChangePasswordAsync(ChangePasswordModel model)
        {
            if (model.MatKhauMoi != model.XacNhanMatKhauMoi)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0010);
            }
            if (string.IsNullOrEmpty(model.Id))
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }
            var user = _sqlContext.User.FirstOrDefault(r => r.Id.Equals(model.Id));
            if (user == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }
            if (!model.IsChange)
            {
                var passwordHash = PasswordUtils.ComputeHash(model.MatKhauCu + user.SecurityStamp);
                if (!user.PasswordHash.Equals(passwordHash))
                {
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0005);
                }
            }

            user.PasswordHash = PasswordUtils.ComputeHash(model.MatKhauMoi + user.SecurityStamp);

            _sqlContext.SaveChangesAsync();
            //xóa cache đăng nhâp của user bị thay đổi pass
            _authService?.RemoveRedis(user.Id);

        }

        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        public async Task<UserInfoModel> GetUserInfoAsnyc(string userId)
        {
            // Lấy thông tin user
            var result = await (from a in _sqlContext.User.AsNoTracking()
                                where a.Id.Equals(userId)
                                select new UserInfoModel()
                                {
                                    Id = a.Id,
                                    UserName = a.UserName,
                                    Avatar = a.Avatar,
                                    Email = a.Email,
                                    //PhoneNumber = a.PhoneNumber,
                                    FullName = a.FullName,
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }

            return result;
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="model">Modle thông tin cập nhật</param>
        /// <param name="userId">Id người cập nhật</param>
        /// <returns></returns>
        public async Task UpdateUserInfoAsync(string id, UserInfoModel model, string userId)
        {
            var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(id));
            if (user == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0009);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                user.UserName = model.UserName?.Trim();
                user.Avatar = model.Avatar;
                user.Email = model.Email?.Trim();
                //user.PhoneNumber = model.PhoneNumber?.Trim();
                user.FullName = model.FullName;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;

                try
                {
                    await _sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}
