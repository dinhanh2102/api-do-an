using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class KyCong
    {
        public string Id { get; set; }
        public string MaKyCong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int? Khoa { get; set; }
        public DateTime? NgayTinhCong { get; set; }
        public double? NgayCongTrongThang { get; set; }
        public int TrangThai { get; set; }
    }
}
