using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class NhanVien_TangCa
    {
        public string Id { get; set; }
        public string IdNhanVien { get; set; }
        public string IdKyCong { get; set; }
        public double? HeSoTangCa { get; set; }
        public double? SoGioTangCa { get; set; }
        public int? NgayTangCa { get; set; }
    }
}
