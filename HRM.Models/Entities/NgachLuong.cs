using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class NgachLuong
    {
        public string Id { get; set; }
        public string TenNgach { get; set; }
        public string HeSo { get; set; }
        public int BacLuong { get; set; }
    }
}
