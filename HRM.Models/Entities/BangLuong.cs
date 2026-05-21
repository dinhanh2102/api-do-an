using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class BangLuong
    {
        public string Id { get; set; }
        public string IdKyCong { get; set; }
        public string MaNhanVien { get; set; }
        public double TienLuong { get; set; }
        public double? UngLuong { get; set; }
        public double? PhuCap { get; set; }
        public double? TangCa { get; set; }
        public int? DiMuon { get; set; }
        public double? LuongCoBan { get; set; }
        public string TenNhanVien { get; set; }
        public string TenChucDanh { get; set; }
        public double? TongNgayCong { get; set; }
        public double? NgayCongThuc { get; set; }
    }
}
