using HarfBuzzSharp;
using HRM.Common;
using HRM.Common.Resource;
using HRM.Models.Entities;
using HRM.Models.Projects.ChamCong;
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
using HRM.Services.Projects.KyCong;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace HRM.Services.Projects.ChamCong
{
    public class ChamCongService : IChamCongService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly IKyCongService _kyCongService;

        public ChamCongService(CoreProjectContext sqlContext, IKyCongService kyCongService)
        {
            _sqlContext = sqlContext;
            _kyCongService = kyCongService;
        }

        /// <summary>
        /// Tìm kiếm kỳ công
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task ChamCong(ChamCongCreateModel model)
        {
            var nhanVien = _sqlContext.NhanVien.FirstOrDefault(s => s.MaNhanVien.Equals(model.MaNhanVien));
            if (nhanVien == null)
            {
                throw HRMException.CreateInstance("Nhân viên không tồn tại");
            }
            var namHienTai = model.ThoiGian.Value.Year;
            var thangHienTai = model.ThoiGian.Value.Month;

            var kyCong = _sqlContext.KyCong.FirstOrDefault(s => s.Thang == thangHienTai && s.Nam.Equals(namHienTai));

            if (kyCong == null)
            {
                HRM.Models.Entities.KyCong kyCongNew = new Models.Entities.KyCong
                {
                    Id = Guid.NewGuid().ToString(),
                    Thang = thangHienTai,
                    Nam = namHienTai,
                    Khoa = 1,
                    MaKyCong = $"KC-{thangHienTai}/{namHienTai}",
                    NgayCongTrongThang = KyCongUtil.demSoNgayLamViecTrongThang(thangHienTai, namHienTai),
                    NgayTinhCong = DateTime.Now,
                    TrangThai = 1

                };
                var kyCongNhanVien = new KyCong_NhanVien
                {
                    Id = Guid.NewGuid().ToString(),
                    IdKyCong = kyCongNew.Id,
                    MaNhanVien = model.MaNhanVien,
                    SoCong = 0,
                    SoNgayCong = 0,
                    SoGioTangCa = 0,
                    SoNgayMuon = 0,
                    SoNgayVang = 0,
                };
                _sqlContext.KyCong_NhanVien.Add(kyCongNhanVien);
                _sqlContext.KyCong.Add(kyCongNew);

                var chamCong = _sqlContext.BangCong.FirstOrDefault(s => s.MaNhanVien.Equals(model.MaNhanVien) && s.Ngay == model.Ngay && s.IdKyCong.Equals(kyCongNew.Id));

                BangCong bangCong = new BangCong
                {
                    Id = Guid.NewGuid().ToString(),
                    IdLoaiCong = Guid.NewGuid().ToString(),
                    MaNhanVien = model.MaNhanVien,
                    Ngay = model.Ngay,
                    IdKyCong = kyCongNew.Id
                };
                if (chamCong == null)
                {
                    bangCong.ThoiGianVao = model.ThoiGian;
                    _sqlContext.BangCong.Add(bangCong);

                }
                else if (chamCong != null)
                {
                    if (chamCong.ThoiGianRa == null)
                    {
                        chamCong.ThoiGianRa = model.ThoiGian;
                    }
                    else
                    {
                        throw HRMException.CreateInstance("Bạn đã chấm công hôm nay rồi");

                    }
                }
            }
            else
            {
                var kyCongNhanVien = _sqlContext.KyCong_NhanVien.FirstOrDefault(s => s.MaNhanVien.Equals(model.MaNhanVien) && s.IdKyCong.Equals(kyCong.Id));
                if (kyCongNhanVien == null)
                {
                    var kyCongNhanVienNew = new KyCong_NhanVien
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdKyCong = kyCong.Id,
                        MaNhanVien = model.MaNhanVien,
                        SoCong = 0,
                        SoNgayCong = 0,
                        SoGioTangCa = 0,
                        SoNgayMuon = 0,
                        SoNgayVang = 0,
                    };
                    _sqlContext.KyCong_NhanVien.Add(kyCongNhanVienNew);
                }
                var chamCong = _sqlContext.BangCong.FirstOrDefault(s => s.MaNhanVien.Equals(model.MaNhanVien) && s.Ngay == model.Ngay && s.IdKyCong.Equals(kyCong.Id));

                BangCong bangCong = new BangCong
                {
                    Id = Guid.NewGuid().ToString(),
                    IdLoaiCong = Guid.NewGuid().ToString(),
                    MaNhanVien = model.MaNhanVien,
                    Ngay = model.Ngay,
                    IdKyCong = kyCong.Id
                };
                if (chamCong == null)
                {
                    bangCong.ThoiGianVao = model.ThoiGian;
                    _sqlContext.BangCong.Add(bangCong);

                }
                else if (chamCong != null)
                {
                    if (chamCong.ThoiGianRa == null)
                    {
                        chamCong.ThoiGianRa = model.ThoiGian;
                    }
                    else
                    {
                        throw HRMException.CreateInstance("Bạn đã chấm công hôm nay rồi");

                    }
                }
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
        public async Task LayDanhSachChamCongTheoThang(BangCongGetModel model)
        {
        }
        public async Task createChamCong(ChamCongCreateHandle model)
        {
            DateTime thoiGianVao = new DateTime(
                model.Nam,
                model.Thang,
                model.Ngay,
                model.GioVao,
                model.PhutVao,
                0
            );
            DateTime thoiGianRa = new DateTime(
               model.Nam,
               model.Thang,
               model.Ngay,
               model.GioRa,
               model.PhutRa,
               0
           );
            BangCong bc = new BangCong
            {
                Id = Guid.NewGuid().ToString(),
                IdKyCong = model.IdKyCong,
                MaNhanVien = model.MaNhanVien,
                Nam = model.Nam,
                Ngay = model.Ngay,
                Thang = model.Thang,
                ThoiGianRa = thoiGianRa,
                ThoiGianVao = thoiGianVao,
                IdLoaiCong = ""
            };

            _sqlContext.BangCong.Add(bc);
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
        public async Task updateChamCong(string id, ChamCongGetModel model)
        {
            var bc = _sqlContext.BangCong.FirstOrDefault(s => s.Id.Equals(id));
            if (bc == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0032);
            }


            DateTime thoiGianVao = new DateTime(
                model.Nam,
                model.Thang,
                model.Ngay,
                model.GioVao,
                model.PhutVao,
                0
            );
            DateTime thoiGianRa = new DateTime(
               model.Nam,
               model.Thang,
               model.Ngay,
               model.GioRa,
               model.PhutRa,
               0
           );
            bc.IdKyCong = model.IdKyCong;
            bc.IdLoaiCong = model.IdLoaiCong;
            bc.MaNhanVien = model.MaNhanVien;
            bc.ThoiGianRa = thoiGianRa;
            bc.ThoiGianVao = thoiGianVao;
            bc.IdLoaiCong = "";


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
        public async Task<ChamCongGetModel> getChamCongDetail(string id)
        {
            var bangCong = _sqlContext.BangCong.FirstOrDefault(s => s.Id == id);

            if (bangCong == null)
            {
                throw HRMException.CreateInstance("Bang cong khong ton tai");
            }
            int nam = 0;
            int thang = 0;
            int gioRa = 0;
            int phutRa = 0;
            if (bangCong.ThoiGianVao != null)
            {
                nam = bangCong.ThoiGianVao.Value.Year;
                thang = bangCong.ThoiGianVao.Value.Month;
            }
            if (bangCong.ThoiGianRa != null)
            {
                gioRa = bangCong.ThoiGianRa.Value.Hour;
                phutRa = bangCong.ThoiGianRa.Value.Minute;

            }
            try
            {
                ChamCongGetModel chamCong = new ChamCongGetModel
                {
                    Id = bangCong.Id,
                    IdKyCong = bangCong.IdKyCong,
                    IdLoaiCong = bangCong.IdLoaiCong,
                    MaNhanVien = bangCong.MaNhanVien,
                    ThoiGianRa = bangCong.ThoiGianRa,
                    ThoiGianVao = bangCong.ThoiGianVao,
                    Ngay = bangCong.Ngay,
                    GioVao = bangCong.ThoiGianVao.Value.Hour,
                    GioRa = gioRa,
                    PhutVao = bangCong.ThoiGianVao.Value.Minute,
                    PhutRa = phutRa,
                    Thang = thang,
                    Nam = nam,
                };
                return chamCong;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public async Task<NhanVienSearchResultModel> getNhanVienTheoMaNhanVien(string maNhanVien)
        {
            var nhanVien = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(x => x.MaNhanVien.Equals(maNhanVien));
            if (nhanVien == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            return new NhanVienSearchResultModel
            {
                Id = nhanVien.Id,
                MaNhanVien = nhanVien.MaNhanVien,
                TenNhanVien = nhanVien.TenNhanVien,
                SoDienThoai = nhanVien.SoDienThoai,
                Email = nhanVien.Email,
                DiaChi = nhanVien.DiaChi,
                UserId = nhanVien.UserId,
                DOB = nhanVien.DOB,
                CanCuocCongDan = nhanVien.CanCuocCongDan,
                IdChucDanh = nhanVien.IdChucDanh,
                SoTKNganHang = nhanVien.SoTKNganHang,
                //NganHang = nhanVien.NganHang,
                GioiTinh = nhanVien.GioiTinh,
                IdTrinhDo = nhanVien.IdTrinhDo,
                Avatar = nhanVien.Avatar,
            };
        }

    }


}
