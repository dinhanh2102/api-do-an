using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class UngLuong
    {
        public string Id { get; set; }
        public int Nam { get; set; }
        public int Thang { get; set; }
        public int Ngay { get; set; }
        public double? SoTien { get; set; }
        public bool TrangThai { get; set; }
        public string IdNhanVien { get; set; }
    }
}
