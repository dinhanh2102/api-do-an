using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class ChuyenNganhHoc
    {
        public string Id { get; set; }
        public string TenChuyenNganh { get; set; }
        public string IdTruongHoc { get; set; }
    }
}
