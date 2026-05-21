using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class BangCong
    {
        public string Id { get; set; }
        public int Ngay { get; set; }
        public DateTime? ThoiGianVao { get; set; }
        public DateTime? ThoiGianRa { get; set; }
        public string MaNhanVien { get; set; }
        public string IdLoaiCong { get; set; }
        public string IdKyCong { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
    }
}
