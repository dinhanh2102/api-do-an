using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class TruongHoc
    {
        public string Id { get; set; }
        public string TenTruongHoc { get; set; }
        public string MaTruong { get; set; }
        public string DiaChi { get; set; }
    }
}
