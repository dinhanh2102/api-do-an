using HRM.Common;
using HRM.Common.Resource;
using HRM.Common.Utils;
using HRM.Models.Cores.Function;
using HRM.Models.Entities;
using HRM.Models.Projects.NhanVien;
using HRM.Services.Cores.Users;
using HRM.Common.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Syncfusion.DocIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControl.Core;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace HRM.Services.Projects.NhanVien
{
    public class NhanVienService : INhanVienService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly IUserService _userService;

        public NhanVienService(CoreProjectContext sqlContext, IUserService userService)
        {
            _sqlContext = sqlContext;
            _userService = userService;
        }

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<NhanVienSearchResultModel>> SearchNhanVienAsync(NhanVienSearchModel searchModel, string userId)
        {
            SearchBaseResultModel<NhanVienSearchResultModel> result = new SearchBaseResultModel<NhanVienSearchResultModel>();
            var dataQuery = (from a in _sqlContext.NhanVien.AsNoTracking()
                             join b in _sqlContext.ChucDanh.AsNoTracking() on a.IdChucDanh equals b.Id
                             join c in _sqlContext.PhongBan.AsNoTracking() on b.IdPhongBan equals c.Id
                             join d in _sqlContext.DonVi.AsNoTracking() on c.IdDonVi equals d.Id
                             select new NhanVienSearchResultModel
                             {
                                 Id = a.Id,
                                 MaNhanVien = a.MaNhanVien,
                                 TenNhanVien = a.TenNhanVien,
                                 SoDienThoai = a.SoDienThoai,
                                 Email = a.Email,
                                 DiaChi = a.DiaChi,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 UserId = a.UserId,
                                 DOB = a.DOB,
                                 CanCuocCongDan = a.CanCuocCongDan,
                                 IdChucDanh = a.IdChucDanh,
                                 TenChucDanh = b.TenChucDanh,
                                 IdPhongBan = b.IdPhongBan,
                                 SoTKNganHang = a.SoTKNganHang,
                                 NganHang = a.NganHang,
                                 //TenNganHang = _c.TenNganHang,
                                 IdDonVi = c.IdDonVi,
                                 GioiTinh = a.GioiTinh,
                                 IdTrinhDo = a.IdTrinhDo,
                                 //TenTrinhDo = _d.Name,
                                 TenPhongBan = c.TenPhongBan,
                                 TenBaoHiem = d.TenDonVi
                             }).ToList();
            PhongBan phongBanHienTai = null;
            DonVi donViHienTai = null;
            var phongBan = _sqlContext.PhongBan.AsNoTracking().ToList();
            var nhanVienHienTai = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(a => a.UserId.Equals(userId));
            if (nhanVienHienTai != null)
            {
                var chucDanhHienTai = _sqlContext.ChucDanh.AsNoTracking().FirstOrDefault(a => a.Id == nhanVienHienTai.IdChucDanh);
                phongBanHienTai = phongBan.FirstOrDefault(a => a.Id.Equals(chucDanhHienTai.IdPhongBan));
                donViHienTai = _sqlContext.DonVi.AsNoTracking().FirstOrDefault(a => a.Id.Equals(phongBanHienTai.IdDonVi));
                //var data = dataQuery.ToList();
            }

            if (!string.IsNullOrEmpty(searchModel.TenNhanVien))
            {
                dataQuery = dataQuery.Where(x => x.TenNhanVien.Contains(searchModel.TenNhanVien)).ToList();
            }
            if (!string.IsNullOrEmpty(userId))
            {
                var checkRole = _sqlContext.User.AsNoTracking().FirstOrDefault(a => a.Id.Equals(userId));
                var groupUser = _sqlContext.UserGroup.AsNoTracking().FirstOrDefault(a => a.Id.Equals(checkRole.UserGroupId));
                if (groupUser == null)
                {
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0036);

                }
                if (groupUser != null)
                {
                    if (groupUser.Name.ToLower() == "nhân viên")
                    {
                        dataQuery = dataQuery.Where(a => a.UserId == userId).ToList();
                    }
                    else if (groupUser.Name.ToLower() == "trưởng phòng")
                    {

                        dataQuery = dataQuery.Where(a => a.IdDonVi == donViHienTai.Id).ToList();
                        dataQuery = dataQuery.Where(a => a.IdPhongBan == phongBanHienTai.Id).ToList();
                    }
                }


            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.TenNhanVien).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateNhanVienAsync(NhanVienCreateModel model, string userId)
        {
            var checkEmail = _sqlContext.NhanVien.AsNoTracking().Where(a => model.Email.ToLower().Trim().Equals(a.Email.ToLower().Trim())).FirstOrDefault();
            if (checkEmail != null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0007);
            }
            var systemParams = _sqlContext.SystemParams.FirstOrDefault(r => HRMConstants.Ma_Nhan_Vien.Equals(r.ParamName));
            if (systemParams == null)
            {
                systemParams = new SystemParams()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParamName = HRMConstants.Ma_Nhan_Vien,
                    ParamValue = DateTime.Now.Year.ToString(),
                    Index = 1,
                    ControlType = 1,
                    DisplayName = "NV"
                };
                _sqlContext.SystemParams.Add(systemParams);
            }
            else
            {
                if (systemParams.ParamValue != DateTime.Now.Year.ToString())
                {
                    systemParams.ParamValue = DateTime.Now.Year.ToString();
                    systemParams.Index = 0;
                }
            }
            int serialMax = systemParams.Index + 1;
            model.MaNhanVien = string.Concat(systemParams.DisplayName, serialMax.ToString("00000"));

            #region Tạo tài khoản
            //Xử lý khoảng trống
            model.MaNhanVien = model.MaNhanVien?.Trim();
            model.Email = model.Email?.Trim();
            model.SoDienThoai = model.SoDienThoai?.Trim();

            //Kiểm tra tồn tại tài khoản
            var userName = _sqlContext.User.AsNoTracking().FirstOrDefault(a => a.UserName.Equals(model.MaNhanVien));
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
            string pass = "NhanVien@123";
            var nhomNhanVien = _sqlContext.UserGroup.FirstOrDefault(s => s.Name.ToLower() == "nhân viên");
            
            User user = new User
            {
                Id = model.MaNhanVien,
                UserName = model.MaNhanVien,
                Email = model.Email,
                SecurityStamp = PasswordUtils.CreateSecurityStamp(),
                LockoutEnabled = false,
                //UserGroupId = "NV",
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
                FullName = model.TenNhanVien,
            };
            if (nhomNhanVien != null)
            {
                user.UserGroupId = nhomNhanVien.Id;
            }
            var nhomNguoiDung = _sqlContext.UserGroup.AsNoTracking().FirstOrDefault(a => a.Id.Equals("NV"));
            user.PasswordHash = PasswordUtils.ComputeHash($"{pass}{user.SecurityStamp}");
            _sqlContext.User.Add(user);

            // Thêm mới bảng quyền
            List<UserPermission> userPermissions = new List<UserPermission>();
            UserPermission userPermission = null;
            var listFunction = _sqlContext.UserGroupFunction.AsNoTracking().Where(e => e.UserGroupId.Equals("NV")).ToList();
            foreach (var item in listFunction)
            {
                userPermission = new UserPermission();
                userPermission.Id = Guid.NewGuid().ToString();
                userPermission.UserId = user.Id;
                userPermission.FunctionId = item.FunctionId;
                userPermissions.Add(userPermission);
            }
            #endregion

            Models.Entities.NhanVien nhanVien = new Models.Entities.NhanVien()
            {
                Id = Guid.NewGuid().ToString(),
                TenNhanVien = model.TenNhanVien,
                MaNhanVien = model.MaNhanVien,
                SoDienThoai = model.SoDienThoai,
                Email = model.Email,
                DiaChi = model.DiaChi,
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
                UserId = model.MaNhanVien,
                DOB = model.DOB,
                CanCuocCongDan = model.CanCuocCongDan,
                IdChucDanh = model.IdChucDanh,
                SoTKNganHang = model.SoTKNganHang,
                NganHang = model.NganHang,
                GioiTinh = model.GioiTinh,
                IdTrinhDo = model.IdTrinhDo,
                Avatar = model.Avatar,
            };
            _sqlContext.NhanVien.Add(nhanVien);

            //if (!string.IsNullOrEmpty(model.FilePath))
            //{
            //    TaiLieuDinhKem taiLieuDinhKem = new TaiLieuDinhKem()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        ObjectId = nhanVien.Id,
            //        FileName = model.FileName,
            //        FilePath = model.FilePath,
            //        FileSize = model.FileSize.Value,
            //        Extention = string.Empty,
            //        HashValue = string.Empty,
            //        CreateBy = userId,
            //        CreateDate = DateTime.Now,
            //        UpdateBy = userId,
            //        UpdateDate = DateTime.Now,
            //    };
            //    _sqlContext.TaiLieuDinhKem.Add(taiLieuDinhKem);
            //}
            systemParams.Index = int.Parse(model.MaNhanVien.Substring(Math.Max(0, model.MaNhanVien.Length - 5)));

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
                    //if (!string.IsNullOrEmpty(model.FilePath))
                    //{
                    //    if (File.Exists(model.FilePath))
                    //        File.Delete(model.FilePath);
                    //}
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xoá nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteNhanVienAsync(string id, string userId)
        {
            var nhanVien = _sqlContext.NhanVien.FirstOrDefault(e => id.Equals(e.Id));
            if (nhanVien != null)
            {
                string UserId = nhanVien.UserId;
                await _userService.DeleteUserAsync(UserId, userId);

            }

            var bangLuongNhanVien = _sqlContext.BangLuong.Where(s => s.MaNhanVien == nhanVien.MaNhanVien).ToList();
            if (bangLuongNhanVien.Count > 0)
            {
                _sqlContext.BangLuong.RemoveRange(bangLuongNhanVien);
            }
            var kyCongNhanVien = _sqlContext.BangCong.Where(s => s.MaNhanVien == nhanVien.MaNhanVien).ToList();
            if (kyCongNhanVien.Count > 0)
            {
                _sqlContext.BangCong.RemoveRange(kyCongNhanVien);
            }
            _sqlContext.NhanVien.Remove(nhanVien);
            //var userNhanVien = _sqlContext.User.FirstOrDefault(e => e.Id.Equals(UserId));

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
        /// Lấy nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NhanVienSearchResultModel> GetNhanVienByIdAsync(string id)
        {
            var nhanVien = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (nhanVien == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            var query = (from a in _sqlContext.NhanVien.AsNoTracking()
                         join b in _sqlContext.ChucDanh.AsNoTracking() on a.IdChucDanh equals b.Id
                         join c in _sqlContext.PhongBan.AsNoTracking() on b.IdPhongBan equals c.Id
                         where a.Id.Equals(id)
                         select new NhanVienSearchResultModel
                         {
                             Id = a.Id,
                             MaNhanVien = a.MaNhanVien,
                             TenNhanVien = a.TenNhanVien,
                             SoDienThoai = a.SoDienThoai,
                             Email = a.Email,
                             DiaChi = a.DiaChi,
                             UserId = a.UserId,
                             DOB = a.DOB,
                             CanCuocCongDan = a.CanCuocCongDan,
                             IdChucDanh = a.IdChucDanh,
                             TenChucDanh = b.TenChucDanh,
                             SoTKNganHang = a.SoTKNganHang,
                             NganHang = a.NganHang,
                             GioiTinh = a.GioiTinh,
                             IdTrinhDo = a.IdTrinhDo,
                             IdDonVi = c.IdDonVi,
                             IdPhongBan = b.IdPhongBan,
                             Avatar = a.Avatar,
                         }).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateNhanVienAsync(string id, NhanVienCreateModel model, string userId)
        {
            var nhanVien = _sqlContext.NhanVien.FirstOrDefault(x => x.Id.Equals(id));
            if (nhanVien == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }

            var fileAnh = nhanVien.Avatar;
            nhanVien.TenNhanVien = model.TenNhanVien;
            nhanVien.SoDienThoai = model.SoDienThoai;
            nhanVien.Email = model.Email;
            nhanVien.DiaChi = model.DiaChi;
            nhanVien.UpdateBy = userId;
            nhanVien.UpdateDate = DateTime.Now;
            nhanVien.DOB = model.DOB;
            nhanVien.CanCuocCongDan = model.CanCuocCongDan;
            nhanVien.Avatar = model.Avatar;
            nhanVien.IdChucDanh = model.IdChucDanh;
            nhanVien.SoTKNganHang = model.SoTKNganHang;
            nhanVien.NganHang = model.NganHang;
            nhanVien.GioiTinh = model.GioiTinh;
            nhanVien.IdTrinhDo = model.IdTrinhDo;

            var user = _sqlContext.User.FirstOrDefault(x => x.Id.Equals(nhanVien.UserId));
            if (user != null)
            {
                user.Email = model.Email;
                user.FullName = model.TenNhanVien;
                user.Avatar = model.Avatar;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;
            }

            //var fileAnh = _sqlContext.TaiLieuDinhKem.FirstOrDefault(a => a.ObjectId.Equals(id));

            //if (!string.IsNullOrEmpty(model.FilePath) && fileAnh?.FilePath != (model.FilePath))
            //{
            //    if (fileAnh != null)
            //    {
            //        _sqlContext.TaiLieuDinhKem.Remove(fileAnh);
            //    }
            //    TaiLieuDinhKem taiLieuDinhKem = new TaiLieuDinhKem()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        ObjectId = id,
            //        FileName = model.FileName,
            //        FilePath = model.FilePath,
            //        FileSize = model.FileSize.Value,
            //        Extention = string.Empty,
            //        HashValue = string.Empty,
            //        CreateBy = userId,
            //        CreateDate = DateTime.Now,
            //        UpdateBy = userId,
            //        UpdateDate = DateTime.Now,
            //    };
            //    _sqlContext.TaiLieuDinhKem.Add(taiLieuDinhKem);

            //    if (!string.IsNullOrEmpty(nhanVien.UserId))
            //    {
            //        if (user != null)
            //        {
            //            user.Avatar = taiLieuDinhKem.FilePath;
            //        }
            //    }
            //}

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    if (!string.IsNullOrEmpty(fileAnh) && !string.IsNullOrEmpty(model.Avatar))
                    {
                        if (File.Exists(fileAnh))
                            File.Delete(fileAnh);
                    }
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
}
