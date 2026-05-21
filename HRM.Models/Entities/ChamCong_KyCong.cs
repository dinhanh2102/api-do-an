using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class ChamCong_KyCong
    {
        public string Id { get; set; }
        public int? NgayCong { get; set; }
        public string MaNhanVien { get; set; }
        public string IdBangCong { get; set; }
        public string IdKyCong { get; set; }
    }
}
