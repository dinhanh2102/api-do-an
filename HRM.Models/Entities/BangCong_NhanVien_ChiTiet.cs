using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class BangCong_NhanVien_ChiTiet
    {
        public string Id { get; set; }
        public string IdKyCong { get; set; }
        public string MaCongTy { get; set; }
        public string HoTen { get; set; }
        public string Ngay { get; set; }
        public string Thu { get; set; }
        public string GioVao { get; set; }
        public string GioRa { get; set; }
        public double NgayPhep { get; set; }
        public double CongNgayLe { get; set; }
        public double CongChuNhat { get; set; }
        public string KyHieu { get; set; }
        public string GhiChu { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string IdNhanVien { get; set; }
    }
}
