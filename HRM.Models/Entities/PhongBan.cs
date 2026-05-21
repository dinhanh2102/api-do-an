using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class PhongBan
    {
        public string Id { get; set; }
        public string IdDonVi { get; set; }
        public string TenPhongBan { get; set; }
        public string MoTa { get; set; }
        public bool? hasTruongPhong { get; set; }
    }
}
