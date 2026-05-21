using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class ChucDanh
    {
        public string Id { get; set; }
        public string TenChucDanh { get; set; }
        public string IdPhongBan { get; set; }
        public double LuongCoBan { get; set; }
    }
}
