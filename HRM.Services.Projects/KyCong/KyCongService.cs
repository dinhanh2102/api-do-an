using HarfBuzzSharp;
using HRM.Common;
using HRM.Common.Resource;
using HRM.Models.Entities;
using HRM.Models.Projects.KyCong;
using HRM.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;
using HRM.Common;
using HRM.Common.Utils;
using System.Collections.Generic;
using HRM.Models.Projects.NhanVien;
using HRM.Services.Projects.NhanVien;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Cryptography.X509Certificates;

namespace HRM.Services.Projects.KyCong
{
    public class KyCongService : IKyCongService
    {
        private readonly CoreProjectContext _sqlContext;

        public KyCongService(CoreProjectContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm kỳ công
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<KyCongSearchResultModel>> SearchKyCongAsync(KyCongSearchModel searchModel)
        {
            SearchBaseResultModel<KyCongSearchResultModel> result = new SearchBaseResultModel<KyCongSearchResultModel>();
            var dataQuery = (from a in _sqlContext.KyCong.AsNoTracking()
                             select new KyCongSearchResultModel
                             {
                                 Id = a.Id,

                                 MaKyCong = a.MaKyCong,
                                 Thang = a.Thang,
                                 Nam = a.Nam,
                                 Khoa = a.Khoa,
                                 NgayTinhCong = a.NgayTinhCong,
                                 NgayCongTrongThang = a.NgayCongTrongThang,
                                 TrangThai = a.TrangThai
                             }).AsQueryable();
            var data = dataQuery.ToList();
            if (!string.IsNullOrEmpty(searchModel.TenKyCong))
            {
                dataQuery = dataQuery.Where(x => x.MaKyCong.Contains(searchModel.MaKyCong));
            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.MaKyCong).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        /// <summary>
        /// Thêm mới kỳ công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateKyCongAsync(KyCongCreateModel model)
        {
            HRM.Models.Entities.KyCong kyCong = new HRM.Models.Entities.KyCong
            {
                Id = Guid.NewGuid().ToString(),
                MaKyCong = model.MaKyCong,
                Thang = model.Thang,
                Nam = model.Nam,
                Khoa = model.Khoa,
                NgayTinhCong = model.NgayTinhCong,
                NgayCongTrongThang = KyCongUtil.demSoNgayLamViecTrongThang(model.Thang, model.Nam),
                TrangThai = model.TrangThai,
            };


            _sqlContext.KyCong.Add(kyCong);
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
        /// Xoá kỳ công
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteKyCongAsync(string id, string kyCongId)
        {
            var kyCong = _sqlContext.KyCong.FirstOrDefault(e => id.Equals(e.Id));
            if (kyCong == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }

            _sqlContext.KyCong.Remove(kyCong);

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
        /// Lấy kỳ công theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<KyCongSearchResultModel> GetKyCongByIdAsync(string id)
        {
            var kyCong = _sqlContext.KyCong.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (kyCong == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            return new KyCongSearchResultModel()
            {
                Id = kyCong.Id,
                MaKyCong = kyCong.MaKyCong,
                Thang = kyCong.Thang,
                Nam = kyCong.Nam,
                Khoa = kyCong.Khoa,
                NgayTinhCong = kyCong.NgayTinhCong,
                NgayCongTrongThang = kyCong.NgayCongTrongThang,
                TrangThai = kyCong.TrangThai,
            };
        }

        /// <summary>
        /// Cập nhật kỳ công
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateKyCongAsync(string id, KyCongCreateModel model, string kyCongId)
        {
            var kyCong = _sqlContext.KyCong.FirstOrDefault(x => x.Id.Equals(id));
            if (kyCong == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            kyCong.MaKyCong = model.MaKyCong;
            kyCong.Thang = model.Thang;
            kyCong.Nam = model.Nam;
            kyCong.Khoa = model.Khoa;
            kyCong.NgayTinhCong = model.NgayTinhCong;
            kyCong.NgayCongTrongThang = KyCongUtil.demSoNgayLamViecTrongThang(model.Thang, model.Nam);
            kyCong.TrangThai = model.TrangThai;

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


        public async Task PhatSinhKyCongChiTiet(PhatSinhKyCongCreate model)
        {

            var listNhanVien = _sqlContext.NhanVien.ToList();

            List<CongChiTietResult> congChiTiets = new List<CongChiTietResult>();
            ModelCongChiTietResult congChiTietResult = new ModelCongChiTietResult();
            try
            {
                var dataQuery = (from a in _sqlContext.NhanVien.AsNoTracking()
                                 join b in _sqlContext.KyCong_NhanVien.AsNoTracking() on a.MaNhanVien equals b.MaNhanVien
                                 join c in _sqlContext.BangCong.AsNoTracking() on a.MaNhanVien equals c.MaNhanVien
                                 join d in _sqlContext.KyCong.AsNoTracking() on b.IdKyCong equals d.Id
                                 where b.IdKyCong.Equals(model.IdKyCong)
                                 select new
                                 {
                                     a.MaNhanVien,
                                     a.TenNhanVien,
                                     IdKyCong = b.IdKyCong,
                                     IdUser = a.UserId,
                                     IdBangCong = c.Id,
                                     c.Ngay,
                                     d.Thang,
                                     d.Nam,
                                     c.ThoiGianRa,
                                     c.ThoiGianVao,
                                     b.SoNgayCong,
                                     b.SoCong,
                                     b.SoNgayMuon,


                                 }).ToList();


                if (dataQuery.Count > 0)
                {
                    var kyCong = dataQuery.FirstOrDefault();
                    var group = dataQuery.GroupBy(s => s.MaNhanVien).ToList();
                    CongChiTietResult congTheoNhanVien = null;

                    //Get header
                    List<NgayHeader> listNgayHeader = new List<NgayHeader>();

                    for (int j = 1; j <= KyCongUtil.GetDayNumber(kyCong.Thang, kyCong.Nam); j++)
                    {
                        listNgayHeader.Add(new NgayHeader
                        {
                            Ngay = j,
                            Thu = KyCongUtil.LayThuTrongTuan(kyCong.Nam, kyCong.Thang, j),
                        });
                    }
                    congChiTietResult.ngayHeaders = listNgayHeader;
                    congChiTietResult.Title = $"Chi tiết bảng công {kyCong.Thang} - {kyCong.Nam}";

                    var listKyCongNhanVien = _sqlContext.KyCong_NhanVien.Where(s => s.IdKyCong == kyCong.IdKyCong).ToList();
                    foreach (var item in group)
                    {
                        var kyConNV = listKyCongNhanVien.FirstOrDefault(s => s.IdKyCong.Equals(kyCong.IdKyCong) && s.MaNhanVien.Equals(item.Key));

                        var nhanVien = dataQuery.FirstOrDefault(s => s.MaNhanVien.Equals(item.Key));
                        congTheoNhanVien = new CongChiTietResult();
                        congTheoNhanVien.MaNhanVien = nhanVien.MaNhanVien;
                        congTheoNhanVien.TenNhanVien = nhanVien.TenNhanVien;
                        congTheoNhanVien.SoNgayCong = nhanVien.SoNgayCong;
                        var listNgay = (from a in dataQuery
                                        where a.MaNhanVien.Equals(item.Key)
                                        select new
                                        {
                                            IdBangCong = a.IdBangCong,
                                            Ngay = a.Ngay,
                                            ThoiGianVao = a.ThoiGianVao,
                                            ThoiGianRa = a.ThoiGianRa
                                        }).OrderBy(s => s.Ngay).ToList();
                        List<NgayChamCong> ngayCong = new List<NgayChamCong>();
                        for (int j = 1; j <= KyCongUtil.GetDayNumber(kyCong.Thang, kyCong.Nam); j++)
                        {
                            ngayCong.Add(new NgayChamCong
                            {
                                Ngay = j
                            });

                        }
                        double soNgayCong = 0;
                        int soNgayVang = 0;
                        int soNgayMuon = 0;
                        double soGioTangCa = 0;
                        foreach (var nc in listNgay)
                        {

                            var ngayCongAdd = new NgayChamCong
                            {
                                IdChamCong = nc.IdBangCong,
                                Ngay = nc.Ngay,
                                ThoiGianRa = nc.ThoiGianRa,
                                ThoiGianVao = nc.ThoiGianVao,

                            };
                            var thoiGianLam = ngayCongAdd.ThoiGianRa - ngayCongAdd.ThoiGianVao;
                            if (thoiGianLam == null || thoiGianLam.Value <= TimeSpan.FromHours(3))
                            {
                                ngayCongAdd.Label = "Chưa chấm công";

                            }
                            else if (ngayCongAdd.ThoiGianRa - ngayCongAdd.ThoiGianVao >= TimeSpan.FromHours(8))
                            {
                                var label = "";
                                if (ngayCongAdd.ThoiGianVao.Value.Hour >= 8 && ngayCongAdd.ThoiGianVao.Value.Minute > 15)
                                {
                                    label = "- Muộn";
                                    soNgayMuon++;
                                }
                                ngayCongAdd.Label = $"Cả ngày {label}";
                                soNgayCong++;

                            }
                            else if (ngayCongAdd.ThoiGianVao.Value.Hour <= 13)
                            {
                                var label = "";
                                if (ngayCongAdd.ThoiGianVao.Value.Hour >= 8 && ngayCongAdd.ThoiGianVao.Value.Minute > 15)
                                {
                                    label = "- Muộn";
                                    soNgayMuon++;

                                }
                                ngayCongAdd.Label = $"Ca sáng {label}";
                                soNgayCong += 0.5;
                            }
                            else if (ngayCongAdd.ThoiGianVao.Value.Hour >= 14)
                            {
                                var label = "";
                                if (ngayCongAdd.ThoiGianVao.Value.Hour >= 13 && ngayCongAdd.ThoiGianVao.Value.Minute > 15)
                                {
                                    label = "- Muộn";
                                    soNgayMuon++;

                                }
                                ngayCongAdd.Label = $"Ca chiều {label}";
                                soNgayCong += 0.5;
                            }
                            //if (ngayCongAdd.ThoiGianRa.Value.Hour >= 17)
                            //{
                            //    soGioTangCa += ngayCongAdd.ThoiGianRa.Value.Hour - 17;
                            //}
                            ngayCong[nc.Ngay - 1] = ngayCongAdd;
                            string ngayInfo = KyCongUtil.LayThuTrongTuan(kyCong.Nam, kyCong.Thang, nc.Ngay);
                            //if (soGioTangCa > 0)
                            //{

                            //    NhanVien_TangCa nhanVien_TangCa = new NhanVien_TangCa
                            //    {
                            //        Id = Guid.NewGuid().ToString(),
                            //        IdNhanVien = nhanVien.IdNhanVien,
                            //        IdKyCong = kyCong.IdKyCong,
                            //        NgayTangCa = nc.Ngay
                            //    };
                            //    if (ngayInfo == "Chủ nhật")
                            //    {
                            //        nhanVien_TangCa.SoGioTangCa = soGioTangCa;
                            //        nhanVien_TangCa.HeSoTangCa = HRMConstants.TangCaChuNhat;
                            //    }
                            //    else
                            //    {
                            //        nhanVien_TangCa.SoGioTangCa = soGioTangCa;
                            //        nhanVien_TangCa.HeSoTangCa = 2;
                            //    }
                            //    _sqlContext.NhanVien_TangCa.Add(nhanVien_TangCa);
                            //}
                        }


                        congTheoNhanVien.SoNgayCong = soNgayCong;
                        congTheoNhanVien.ChamCongList = ngayCong;
                        congTheoNhanVien.SoNgayMuon = soNgayMuon;


                        var kcNv = listKyCongNhanVien.FirstOrDefault(s => s.MaNhanVien.Equals(item.Key));

                        var kyConChiTietNhanVien = _sqlContext.KyCong_NhanVien.FirstOrDefault(s => s.IdKyCong.Equals(kyCong.IdKyCong) && s.MaNhanVien.Equals(item.Key));

                        kyConChiTietNhanVien.SoNgayCong = soNgayCong;
                        kyConChiTietNhanVien.SoNgayMuon = soNgayMuon;
                        kyConChiTietNhanVien.SoCong = KyCongUtil.demSoNgayLamViecTrongThang(kyCong.Thang, kyCong.Nam);

                        congChiTiets.Add(congTheoNhanVien);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            congChiTietResult.CongChiTietResults = congChiTiets;

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

        public async Task<KyCongResultModel> getListKyCongChiTiet(string idKyCong)
        {
            KyCongResultModel kyCongResult = new KyCongResultModel();


            return kyCongResult;

        }


        private static List<NgayHeader> GetThuTrongTuan(int nam, int thang)
        {
            var listDay = new List<NgayHeader>();
            for (int j = 1; j <= KyCongUtil.GetDayNumber(thang, nam); j++)
            {
                var thuTrongTuan = KyCongUtil.LayThuTrongTuan(nam, thang, j);
                var ngayHeader = new NgayHeader
                {
                    Ngay = j,
                    Thu = thuTrongTuan,
                };

                listDay.Add(ngayHeader);
            }
            return listDay;

        }

        public async Task<ModelCongChiTietResult> getListChamCong(string idKyCong, string userId)
        {

            var userNhanVien = _sqlContext.NhanVien.FirstOrDefault(s => s.UserId.Equals(userId));


            List<CongChiTietResult> congChiTiets = new List<CongChiTietResult>();
            ModelCongChiTietResult congChiTietResult = new ModelCongChiTietResult();
            var dataQuery = (from a in _sqlContext.NhanVien.AsNoTracking()
                             join e in _sqlContext.ChucDanh.AsNoTracking() on a.IdChucDanh equals e.Id
                             join f in _sqlContext.PhongBan.AsNoTracking() on e.IdPhongBan equals f.Id
                             join g in _sqlContext.DonVi.AsNoTracking() on f.IdDonVi equals g.Id
                             join b in _sqlContext.KyCong_NhanVien.AsNoTracking() on a.MaNhanVien equals b.MaNhanVien
                             join c in _sqlContext.BangCong.AsNoTracking() on a.MaNhanVien equals c.MaNhanVien
                             join d in _sqlContext.KyCong.AsNoTracking() on b.IdKyCong equals d.Id
                             where b.IdKyCong.Equals(idKyCong)
                             select new
                             {
                                 a.MaNhanVien,
                                 a.TenNhanVien,
                                 IdKyCong = b.IdKyCong,
                                 IdUser = a.UserId,
                                 IdBangCong = c.Id,
                                 c.Ngay,
                                 d.Thang,
                                 d.Nam,
                                 c.ThoiGianRa,
                                 c.ThoiGianVao,
                                 b.SoNgayCong,
                                 b.SoCong,
                                 b.SoNgayMuon,
                                 IdPhongBan = f.Id,
                                 IdDonVi = g.Id,

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
            if (!string.IsNullOrEmpty(userId))
            {
                var nhanVien = _sqlContext.NhanVien.AsNoTracking().Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                var check = _sqlContext.User.AsNoTracking().Where(a => a.Id.Equals(userId)).FirstOrDefault();
                var groupUser = _sqlContext.UserGroup.AsNoTracking().FirstOrDefault(a => a.Id.Equals(check.UserGroupId));
                if(groupUser == null)
                {
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0036);

                }
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
           
            var kyCong = _sqlContext.KyCong.FirstOrDefault(s => s.Id.Equals(idKyCong));
            //Get header
            List<NgayHeader> listNgayHeader = new List<NgayHeader>();

            for (int j = 1; j <= KyCongUtil.GetDayNumber(kyCong.Thang, kyCong.Nam); j++)
            {
                listNgayHeader.Add(new NgayHeader
                {
                    Ngay = j,
                    Thu = KyCongUtil.LayThuTrongTuan(kyCong.Nam, kyCong.Thang, j),
                });
            }
            congChiTietResult.SoNgayTrongThang = listNgayHeader.Count();
            congChiTietResult.ngayHeaders = listNgayHeader;
            congChiTietResult.Title = $"Chi tiết bảng công tháng {kyCong.Thang} - {kyCong.Nam}";
            congChiTietResult.Thang = kyCong.Thang;
            congChiTietResult.Nam = kyCong.Nam;
            if (dataQuery.Count > 0)
            {
                var group = dataQuery.GroupBy(s => s.MaNhanVien).ToList();
                CongChiTietResult congTheoNhanVien = null;



                foreach (var item in group)
                {
                    var nhanVien = dataQuery.FirstOrDefault(s => s.MaNhanVien.Equals(item.Key));
                    congTheoNhanVien = new CongChiTietResult();
                    congTheoNhanVien.MaNhanVien = nhanVien.MaNhanVien;
                    congTheoNhanVien.TenNhanVien = nhanVien.TenNhanVien;
                    congTheoNhanVien.SoNgayCong = nhanVien.SoNgayCong;
                    var listNgay = (from a in dataQuery
                                    where a.MaNhanVien.Equals(item.Key)
                                    select new
                                    {
                                        IdBangCong = a.IdBangCong,
                                        Ngay = a.Ngay,
                                        ThoiGianVao = a.ThoiGianVao,
                                        ThoiGianRa = a.ThoiGianRa,

                                    }).OrderBy(s => s.Ngay).ToList();
                    List<NgayChamCong> ngayCong = new List<NgayChamCong>();
                    for (int j = 1; j <= KyCongUtil.GetDayNumber(kyCong.Thang, kyCong.Nam); j++)
                    {
                        ngayCong.Add(new NgayChamCong
                        {
                            Ngay = j
                        });

                    }

                    foreach (var nc in listNgay)
                    {

                        var ngayCongAdd = new NgayChamCong
                        {
                            IdChamCong = nc.IdBangCong,
                            Ngay = nc.Ngay,
                            ThoiGianRa = nc.ThoiGianRa,
                            ThoiGianVao = nc.ThoiGianVao,

                        };
                        var thoiGianLam = ngayCongAdd.ThoiGianRa - ngayCongAdd.ThoiGianVao;
                        if (thoiGianLam == null)
                        {
                            ngayCongAdd.Label = "Chưa chấm công";

                        }
                        else if (ngayCongAdd.ThoiGianRa - ngayCongAdd.ThoiGianVao >= TimeSpan.FromHours(8))
                        {
                            var label = "";
                            if (ngayCongAdd.ThoiGianVao.Value.Hour >= 8 && ngayCongAdd.ThoiGianVao.Value.Minute > 15)
                            {
                                label = "- Muộn";
                                //soNgayMuon++;
                            }
                            ngayCongAdd.Label = $"Cả ngày {label}";
                            //soNgayCong++;

                        }
                        else if (ngayCongAdd.ThoiGianVao.Value.Hour < 13)
                        {
                            var label = "";
                            if (ngayCongAdd.ThoiGianVao.Value.Hour >= 8 && ngayCongAdd.ThoiGianVao.Value.Minute > 15)
                            {
                                label = "- Muộn";
                                //soNgayMuon++;

                            }
                            ngayCongAdd.Label = $"Ca sáng {label}";
                            //soNgayCong += 0.5;
                        }
                        else if (ngayCongAdd.ThoiGianVao.Value.Hour >= 13)
                        {
                            var label = "";
                            if (ngayCongAdd.ThoiGianVao.Value.Hour >= 13 && ngayCongAdd.ThoiGianVao.Value.Minute > 15)
                            {
                                label = "- Muộn";
                                //soNgayMuon++;

                            }
                            ngayCongAdd.Label = $"Ca chiều {label}";
                            //soNgayCong += 0.5;
                        }

                        ngayCong[nc.Ngay - 1] = ngayCongAdd;
                    }
                    congTheoNhanVien.SoNgayCong = nhanVien.SoNgayCong;
                    congTheoNhanVien.ChamCongList = ngayCong;
                    //congTheoNhanVien.SoNgayMuon = nhanVien.SoNgayMuon;


                    congChiTiets.Add(congTheoNhanVien);
                }
            }
            congChiTietResult.CongChiTietResults = congChiTiets;

            return congChiTietResult;


        }

        /// <summary>
        /// Xoá kỳ công
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteChamCongAsync(string id)
        {
            var bangCong = _sqlContext.BangCong.FirstOrDefault(e => id.Equals(e.Id));
            if (bangCong == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }

            _sqlContext.BangCong.Remove(bangCong);

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



