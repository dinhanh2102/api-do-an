using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class DonVi
    {
        public string Id { get; set; }
        public string TenDonVi { get; set; }
        public string MaDonVi { get; set; }
        public string ParentId { get; set; }
    }
}
