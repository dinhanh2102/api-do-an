using HRM.Models.Entities;
using Newtonsoft.Json;
using HRM.Common;
using HRM.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HRM.Models.Cores.InitData;

namespace HRM.Api.AppInitialize
{
    public static class InitSystemData
    {
        public static void Init(CoreProjectContext _sqlContext)
        {
            try
            {
                // Thêm mới người dùng fix
                AddUserAndGroup(_sqlContext);

                // Thêm menu và quyền phần mềm
                AddMenuAndPermission(_sqlContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Thêm mới người dùng fix
        /// </summary>
        /// <param name="_sqlContext"></param>
        private static void AddUserAndGroup(CoreProjectContext _sqlContext)
        {
            if (!_sqlContext.User.Any())
            {
                using (var trans = _sqlContext.Database.BeginTransaction())
                {
                    try
                    {
                        //thêm nhóm người dùng mặc định
                        UserGroup groupuser = new UserGroup()
                        {
                            Id = HRMConstants.GroupAdminId,
                            Name = "Quản trị hệ thống",
                            Description = "Nhóm quyền người dùng quản trị hệ thống",
                            //Type = (int)HRMConstants.TypeUser.Manage
                        };
                        _sqlContext.UserGroup.Add(groupuser);

                        //thêm tài khoản mặc định root
                        User userRoot = new User()
                        {
                            Id = HRMConstants.IdUserRootFix,
                            UserName = "root",
                            FullName = "Root",
                            LockoutEnabled = false,
                            SecurityStamp = PasswordUtils.CreateSecurityStamp(),
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            UserGroupId = HRMConstants.GroupAdminId,
                            //Type = (int)HRMConstants.TypeUser.Manage
                        };
                        userRoot.PasswordHash = PasswordUtils.ComputeHash(userRoot.UserName + userRoot.SecurityStamp);
                        _sqlContext.User.Add(userRoot);

                        //thêm tài khoản mặc định admin
                        User userAdmin = new User()
                        {
                            Id = HRMConstants.IdUserAdminFix,
                            UserName = "admin",
                            FullName = "Admin",
                            LockoutEnabled = false,
                            SecurityStamp = PasswordUtils.CreateSecurityStamp(),
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            UserGroupId = HRMConstants.GroupAdminId,
                            //Type = (int)HRMConstants.TypeUser.Manage
                        };
                        userAdmin.PasswordHash = PasswordUtils.ComputeHash(userAdmin.UserName + userAdmin.SecurityStamp);
                        _sqlContext.User.Add(userAdmin);

                        _sqlContext.SaveChanges();
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

        /// <summary>
        /// Thêm menu và quyền phần mềm
        /// </summary>
        /// <param name="_sqlContext"></param>
        private static void AddMenuAndPermission(CoreProjectContext _sqlContext)
        {
            string pathSystemData = Path.Combine(Directory.GetCurrentDirectory(), "InitData/SystemData.json");
            string pathSystemData_LogDateUpdate = Path.Combine(Path.GetDirectoryName(pathSystemData), Path.GetFileNameWithoutExtension(pathSystemData) + "_LogDateUpdate.txt");

            //Lấy thông tin ngày cập nhật file cuối cùng
            string dateUpdateFileSystemData = File.GetLastWriteTime(pathSystemData).ToStringFullDateTime();
            //Thông tin log đọc file lần cuối
            string dateUpdateLast;
            if (File.Exists(pathSystemData_LogDateUpdate))
                dateUpdateLast = File.ReadAllText(pathSystemData_LogDateUpdate);
            else
                dateUpdateLast = DateTime.Now.ToStringFullDateTime();

            //Nếu khới tạo lần đầu tiên hoặc có thay đổi file json thì thực hiện
            if (!_sqlContext.MenuSystem.Any())
            {
                if (File.Exists(pathSystemData))
                {
                    var dataModel = JsonConvert.DeserializeObject<IntDataModel>(File.ReadAllText(pathSystemData));
                    using (var trans = _sqlContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //var listAutoFuncUpdate = _sqlContext.FunctionAuto.ToList();
                            ////Danh sách quyền chưc năng tự động
                            //FunctionAuto functionAutoInput;
                            //FunctionAuto autoFuncUpdate;
                            //foreach (var itemAutoFunc in dataModel.AutoFuncfions)
                            //{
                            //    autoFuncUpdate = listAutoFuncUpdate.Where(r => r.Id.Equals(itemAutoFunc.Id)).FirstOrDefault();

                            //    functionAutoInput = JsonConvert.DeserializeObject<FunctionAuto>(JsonConvert.SerializeObject(itemAutoFunc));
                            //    if (autoFuncUpdate != null)
                            //    {
                            //        _sqlContext.Entry(autoFuncUpdate).CurrentValues.SetValues(itemAutoFunc);
                            //    }
                            //    else
                            //    {
                            //        _sqlContext.FunctionAuto.Add(functionAutoInput);
                            //    }
                            //}

                            var listFunctionUpdate = _sqlContext.Function.ToList();
                            //Danh sách quyền phần mềm
                            Function functionInput;
                            Function functionUpdate;
                            foreach (var itemFunc in dataModel.Funcfions)
                            {
                                functionUpdate = listFunctionUpdate.Where(r => r.Id.Equals(itemFunc.Id)).FirstOrDefault();

                                functionInput = JsonConvert.DeserializeObject<Function>(JsonConvert.SerializeObject(itemFunc));
                                if (functionUpdate != null)
                                {
                                    _sqlContext.Entry(functionUpdate).CurrentValues.SetValues(itemFunc);
                                }
                                else
                                {
                                    _sqlContext.Function.Add(functionInput);
                                }
                            }

                            var listMenuSystemUpdate = _sqlContext.MenuSystem.ToList();
                            //Danh sách menu phần mềm
                            MenuSystem menuInput;
                            MenuSystem menuUpdate;
                            List<string> listDisableMenuId = new List<string>();
                            foreach (var itemMenu in dataModel.Menus)
                            {
                                //Menu bị ẩn
                                if (itemMenu.IsDisable)
                                {
                                    listDisableMenuId.Add(itemMenu.Id);
                                }
                                menuUpdate = listMenuSystemUpdate.Where(r => r.Id.Equals(itemMenu.Id)).FirstOrDefault();

                                menuInput = JsonConvert.DeserializeObject<MenuSystem>(JsonConvert.SerializeObject(itemMenu));
                                if (menuUpdate != null)
                                {
                                    _sqlContext.Entry(menuUpdate).CurrentValues.SetValues(itemMenu);
                                }
                                else
                                {
                                    _sqlContext.MenuSystem.Add(menuInput);
                                }
                            }

                            var listMenuPermissionUpdate = _sqlContext.MenuSystemPermission.ToList();
                            //Danh sách quyền menu
                            MenuSystemPermission menuPermissInput;
                            MenuSystemPermission menuPermissionUpdate;
                            List<string> listDisableFunctionId = new List<string>();
                            foreach (var itemMenu in dataModel.MenuPermissions)
                            {
                                //Các quyền bị ẩn
                                if (listDisableMenuId.Count > 0 && listDisableMenuId.Contains(itemMenu.MenuSystemId))
                                {
                                    listDisableFunctionId.Add(itemMenu.FunctionId);
                                }
                                menuPermissionUpdate = listMenuPermissionUpdate.Where(r => r.Id.Equals(itemMenu.Id)).FirstOrDefault();

                                menuPermissInput = JsonConvert.DeserializeObject<MenuSystemPermission>(JsonConvert.SerializeObject(itemMenu));
                                if (menuPermissionUpdate != null)
                                {
                                    _sqlContext.Entry(menuPermissionUpdate).CurrentValues.SetValues(itemMenu);
                                }
                                else
                                {
                                    _sqlContext.MenuSystemPermission.Add(menuPermissInput);
                                }
                            }

                            //Add quyền cho nhóm và người dùng fix
                            UserGroupFunction groupFunction;
                            UserPermission userPermission;
                            foreach (var itemFunc in dataModel.Funcfions)
                            {
                                //Nếu mà là quyền không bị ẩn
                                if (!listDisableFunctionId.Contains(itemFunc.Id))
                                {
                                    //Quyền nhóm quyền
                                    groupFunction = new UserGroupFunction();
                                    groupFunction.Id = Guid.NewGuid().ToString();
                                    groupFunction.FunctionId = itemFunc.Id;
                                    groupFunction.UserGroupId = HRMConstants.GroupAdminId;
                                    _sqlContext.UserGroupFunction.Add(groupFunction);

                                    //Quyền người dùng
                                    userPermission = new UserPermission();
                                    userPermission.Id = Guid.NewGuid().ToString();
                                    userPermission.FunctionId = itemFunc.Id;
                                    userPermission.UserId = HRMConstants.IdUserAdminFix;
                                    _sqlContext.UserPermission.Add(userPermission);
                                }
                                //Quyền người dùng
                                userPermission = new UserPermission();
                                userPermission.Id = Guid.NewGuid().ToString();
                                userPermission.FunctionId = itemFunc.Id;
                                userPermission.UserId = HRMConstants.IdUserRootFix;
                                _sqlContext.UserPermission.Add(userPermission);
                            }

                            _sqlContext.SaveChanges();
                            trans.Commit();

                            File.WriteAllText(pathSystemData_LogDateUpdate, dateUpdateFileSystemData);
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
    }
}
