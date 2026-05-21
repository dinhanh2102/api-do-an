using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class BangCong_NhanVien
    {
        public string MaNhanVien { get; set; }
        public string Id { get; set; }
        public string IdKyCong { get; set; }
        public string IdBangCong { get; set; }
        public double? SoCong { get; set; }
        public int? TongNgayCong { get; set; }
    }
}
