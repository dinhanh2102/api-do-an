using HarfBuzzSharp;
using HRM.Common;
using HRM.Common.Models;
using HRM.Models.Entities;
using HRM.Models.Projects.BangLuong;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Projects.BangLuong
{
    public class BangLuongService : IBangLuongService
    {
        private readonly CoreProjectContext _sqlContext;
        public BangLuongService(CoreProjectContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm bảng lương
        /// </summary>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<BangLuongSearchResultModel>> SearchBangLuongAsync(BangLuongSearchModel searchModel, string userId)
        {
            SearchBaseResultModel<BangLuongSearchResultModel> result = new SearchBaseResultModel<BangLuongSearchResultModel>();
            try
            {
                List<BangLuongSearchResultModel> listBangLuong = new List<BangLuongSearchResultModel>();
                var kyCong = _sqlContext.KyCong.FirstOrDefault(s => s.Id.Equals(searchModel.IdKyCong));
                if (kyCong == null && searchModel.Thang > 0 && searchModel.Nam > 0)
                {
                    kyCong = _sqlContext.KyCong.FirstOrDefault(s => s.Thang == searchModel.Thang && s.Nam == searchModel.Nam);
                    searchModel.IdKyCong = kyCong?.Id;
                }
                if (kyCong == null)
                {
                    throw HRMException.CreateInstance("Kỳ công không tồn tại");
                }

                var dataQuery = (from a in _sqlContext.BangLuong.AsNoTracking()
                                 join b in _sqlContext.NhanVien.AsNoTracking() on a.MaNhanVien equals b.MaNhanVien
                                 join c in _sqlContext.ChucDanh.AsNoTracking() on b.IdChucDanh equals c.Id
                                 join d in _sqlContext.PhongBan.AsNoTracking() on c.IdPhongBan equals d.Id
                                 join e in _sqlContext.DonVi.AsNoTracking() on d.IdDonVi equals e.Id
                                 where a.IdKyCong.Equals(searchModel.IdKyCong)
                                 select new BangLuongSearchResultModel
                                 {
                                     Id = a.Id,
                                     IdKyCong = a.IdKyCong,
                                     MaNhanVien = a.MaNhanVien,
                                     TenNhanVien = a.TenNhanVien,
                                     TenChucDanh = a.TenChucDanh,
                                     LuongCoBan = a.LuongCoBan,
                                     TienLuong = a.TienLuong,
                                     SoNgayCong = a.NgayCongThuc,
                                     TongSoCong = a.TongNgayCong,
                                     ThangKyCong = kyCong.Thang,
                                     NamKyCong = kyCong.Nam,
                                     SoNgayMuon = a.DiMuon,
                                     SoTienPhuCap = a.PhuCap,
                                     SoGioTangCa = a.TangCa,
                                     SoTienUngLuong = a.UngLuong,
                                     SoTienMuon = a.DiMuon * 50000,
                                     IdDonVi = e.Id,
                                     IdPhongBan = d.Id,
                                 }).ToList();
                var nhanVienHienTai = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(a => a.UserId.Equals(userId));
                PhongBan phongBanHienTai = null;
                DonVi donViHienTai = null;
                if (nhanVienHienTai != null)
                {
                    var chucDanhHienTai = _sqlContext.ChucDanh.AsNoTracking().FirstOrDefault(a => a.Id == nhanVienHienTai.IdChucDanh);
                    phongBanHienTai = _sqlContext.PhongBan.FirstOrDefault(a => a.Id.Equals(chucDanhHienTai.IdPhongBan));
                    donViHienTai = _sqlContext.DonVi.AsNoTracking().FirstOrDefault(a => a.Id.Equals(phongBanHienTai.IdDonVi));
                    //var data = dataQuery.ToList();
                }
                if (!string.IsNullOrEmpty(searchModel.TenNhanVien))
                {
                    dataQuery = dataQuery.Where(s => s.IdNhanVien == searchModel.TenNhanVien).ToList();
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    var nhanVien = _sqlContext.NhanVien.AsNoTracking().Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                    var check = _sqlContext.User.AsNoTracking().Where(a => a.Id.Equals(userId)).FirstOrDefault();
                    var groupUser = _sqlContext.UserGroup.AsNoTracking().FirstOrDefault(a => a.Id.Equals(check.UserGroupId));
                    if (check != null)
                    {
                        if (groupUser.Name.ToLower() == "nhân viên")
                        {
                            dataQuery = dataQuery.Where(x => x.MaNhanVien.Equals(nhanVien.MaNhanVien)).ToList();
                        }
                        else if (groupUser.Name.ToLower() == "trưởng phòng")
                        {
                            dataQuery = dataQuery.Where(a => a.IdDonVi == donViHienTai.Id).ToList();
                            dataQuery = dataQuery.Where(a => a.IdPhongBan == phongBanHienTai.Id).ToList();
                        }
                    }
                }

                result.TotalItems = dataQuery.Count();
                result.Title = $"Bảng lương tháng {kyCong.Thang}/{kyCong.Nam}";
                result.DataResults = dataQuery.OrderBy(o => o.MaNhanVien).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Thêm mới bảng lương
        /// </summary>
        /// <returns></returns>
        public async Task CreateBangLuongAsync(BangLuongCreateModel model, string userId)
        {


            var dataQuery = (from a in _sqlContext.NhanVien.AsNoTracking()
                             join b in _sqlContext.KyCong_NhanVien.AsNoTracking() on a.MaNhanVien equals b.MaNhanVien
                             join c in _sqlContext.KyCong.AsNoTracking() on b.IdKyCong equals c.Id
                             join e in _sqlContext.NhanVien_PhuCap.AsNoTracking() on a.Id equals e.IdNhanVien into pcnv
                             from _e in pcnv.DefaultIfEmpty()
                             join h in _sqlContext.PhuCap.AsNoTracking() on _e.IdPhuCap equals h.Id into pc
                             from _h in pc.DefaultIfEmpty()
                             join f in _sqlContext.TangCa.AsNoTracking() on a.Id equals f.IdNhanVien into tc
                             from _f in tc.DefaultIfEmpty()
                             join g in _sqlContext.UngLuong.AsNoTracking() on a.Id equals g.IdNhanVien into ul
                             from _g in ul.DefaultIfEmpty()
                             join i in _sqlContext.ChucDanh.AsNoTracking() on a.IdChucDanh equals i.Id
                             select new BangLuongSearchResultModel
                             {
                                 IdKyCong = c.Id,
                                 IdNhanVien = a.Id,
                                 MaNhanVien = a.MaNhanVien,
                                 TenNhanVien = a.TenNhanVien,
                                 TenChucDanh = i.TenChucDanh,
                                 LuongCoBan = i.LuongCoBan,

                                 TongSoCong = b.SoCong,
                                 SoNgayCong = b.SoNgayCong,
                                 SoNgayMuon = b.SoNgayMuon,
                                 SoNgayVang = b.SoNgayVang,
                                 ThangKyCong = c.Thang,
                                 NamKyCong = c.Nam,

                                 IdPhuCap = _h.Id,
                                 SoTienPhuCap = _h.SoTien,

                                 SoGioTangCa = _f.SoGio,
                                 IdLoaiCaTangCa = _f.IdLoaiCa,
                                 ThangTangCa = _f.Thang,
                                 NamTangCa = _f.Nam,

                                 SoTienUngLuong = _g.SoTien,
                                 ThangUngLuong = _g.Thang,
                                 NamUngLuong = _g.Nam,

                             }).AsQueryable();

            var kyCong = _sqlContext.KyCong.AsNoTracking().FirstOrDefault(s => s.Id.Equals(model.IdKyCong));
            if (kyCong == null && model.Thang > 0 && model.Nam > 0)
            {
                kyCong = _sqlContext.KyCong.AsNoTracking().FirstOrDefault(s => s.Thang == model.Thang && s.Nam == model.Nam);
                model.IdKyCong = kyCong?.Id;
            }
            if (kyCong == null)
            {
                throw HRMException.CreateInstance("Kỳ công không tồn tại!");
            }
            var bangLuongCu = _sqlContext.BangLuong.Where(a => a.IdKyCong.Equals(kyCong.Id));
            if (model.IdKyCong != null)
            {
                dataQuery = dataQuery.Where(x => x.IdKyCong == model.IdKyCong);
            }


            if (!string.IsNullOrEmpty(userId))
            {
                var nhanVien = _sqlContext.NhanVien.AsNoTracking().Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                var check = _sqlContext.User.AsNoTracking().Where(a => a.Id.Equals(userId)).FirstOrDefault();
                if (check != null)
                {
                    if (check.UserGroupId == "NV")
                        dataQuery = dataQuery.Where(x => x.MaNhanVien.Equals(nhanVien.MaNhanVien));
                }
            }

            var nhanVienGroup = dataQuery.GroupBy(a => a.MaNhanVien).Select(a => new BangLuongSearchResultModel
            {
                MaNhanVien = a.Key,

            }).ToList();

            //var loaiCa
            List<Models.Entities.BangLuong> listBangLuong = new List<Models.Entities.BangLuong>();
            Models.Entities.BangLuong bangLuong = null;
            foreach (var item in nhanVienGroup)
            {
                BangLuongSearchResultModel bangLuongNv = new BangLuongSearchResultModel();
                var nhanVienBangLuong = dataQuery.FirstOrDefault(s => s.MaNhanVien == item.MaNhanVien);


                var luongNgay = nhanVienBangLuong.LuongCoBan / nhanVienBangLuong.TongSoCong;
                var tienLuongTheoCong = luongNgay * nhanVienBangLuong.SoNgayCong;
                int? tienPhatMuon = 0;
                double? tienTangCa = 0;
                double? tienUng = 0;
                double? tienThucNhan = 0;

                tienThucNhan = tienLuongTheoCong;
                if (nhanVienBangLuong.SoNgayMuon != 0)
                {
                    tienPhatMuon = nhanVienBangLuong.SoNgayMuon * 50000;
                    tienThucNhan -= tienPhatMuon;
                }
                if (nhanVienBangLuong.SoGioTangCa != 0 && nhanVienBangLuong.SoGioTangCa != null)
                {
                    tienTangCa = nhanVienBangLuong.SoGioTangCa * 2 * luongNgay;
                    tienThucNhan += tienTangCa;
                }
                if (nhanVienBangLuong.SoTienUngLuong != null && nhanVienBangLuong.SoTienUngLuong != null)
                {
                    tienUng = nhanVienBangLuong.SoTienUngLuong;
                    tienThucNhan -= tienUng;
                }


                bangLuongNv.ThangKyCong = kyCong.Thang;
                bangLuongNv.NamKyCong = kyCong.Nam;
                bangLuongNv.SoNgayCong = nhanVienBangLuong.SoNgayCong;
                bangLuongNv.TongSoCong = nhanVienBangLuong.TongSoCong;
                bangLuongNv.ThangUngLuong = nhanVienBangLuong.ThangUngLuong;
                bangLuongNv.SoTienUngLuong = tienUng;
                bangLuongNv.TienLuong = tienThucNhan;
                bangLuongNv.LuongCoBan = nhanVienBangLuong.LuongCoBan;
                bangLuongNv.TenNhanVien = nhanVienBangLuong.TenNhanVien;
                bangLuongNv.MaNhanVien = nhanVienBangLuong.MaNhanVien;
                bangLuongNv.TenChucDanh = nhanVienBangLuong.TenChucDanh;
                bangLuongNv.SoGioTangCa = nhanVienBangLuong.SoGioTangCa;
                bangLuongNv.SoNgayMuon = nhanVienBangLuong.SoNgayMuon;



                bangLuong = new Models.Entities.BangLuong
                {
                    IdKyCong = model.IdKyCong,
                    Id = Guid.NewGuid().ToString(),
                    MaNhanVien = bangLuongNv.MaNhanVien,
                    DiMuon = nhanVienBangLuong.SoNgayMuon,
                    PhuCap = nhanVienBangLuong.SoTienPhuCap,
                    TangCa = nhanVienBangLuong.SoTienTangCa,
                    UngLuong = nhanVienBangLuong.SoTienUngLuong,
                    LuongCoBan = nhanVienBangLuong.LuongCoBan,
                    NgayCongThuc = nhanVienBangLuong.SoNgayCong,
                    TenChucDanh = nhanVienBangLuong.TenChucDanh,
                    TenNhanVien = nhanVienBangLuong.TenNhanVien,
                    TongNgayCong = nhanVienBangLuong.TongSoCong,


                };

                if (tienThucNhan != 0 && tienThucNhan != null)
                {
                    double tienThucNhanRounded = Math.Round(tienThucNhan.Value / 1000) * 1000;
                    bangLuong.TienLuong = tienThucNhanRounded;
                }
                else
                {
                    bangLuong.TienLuong = 0;
                }
                listBangLuong.Add(bangLuong);

            }
            _sqlContext.BangLuong.RemoveRange(bangLuongCu);
            _sqlContext.BangLuong.AddRange(listBangLuong);
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
}
