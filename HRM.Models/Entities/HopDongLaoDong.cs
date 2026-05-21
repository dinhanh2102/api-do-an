using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class HopDongLaoDong
    {
        public string Id { get; set; }
        public string TenHopDong { get; set; }
        public string ThoiHanHopDong { get; set; }
        public int? HinhThucLamViec { get; set; }
        public DateTime NgayCoHieuLuc { get; set; }
        public int TrangThaiHopDong { get; set; }
        public string SoHopDong { get; set; }
        public DateTime NgayKy { get; set; }
        public int LoaiHopDong { get; set; }
        public string IdNgachLuong { get; set; }
        public string NgayHetHan { get; set; }
        public string GhiChu { get; set; }
        public string IdNhanVien { get; set; }
    }
}
