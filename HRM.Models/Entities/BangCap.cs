using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class BangCap
    {
        public string Id { get; set; }
        public string TenBangCap { get; set; }
        public string IdNhanVien { get; set; }
        public string TenChuyenNganh { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string HinhThucDaoTao { get; set; }
        public string ChungChi { get; set; }
        public string MoTa { get; set; }
    }
}
