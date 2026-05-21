using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class KyCong_NhanVien
    {
        public string Id { get; set; }
        public string MaNhanVien { get; set; }
        public string IdKyCong { get; set; }
        public double SoCong { get; set; }
        public double SoNgayCong { get; set; }
        public int? SoNgayMuon { get; set; }
        public int? SoNgayVang { get; set; }
        public double? SoGioTangCa { get; set; }
    }
}
