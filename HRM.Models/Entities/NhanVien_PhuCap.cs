using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class NhanVien_PhuCap
    {
        public string Id { get; set; }
        public string IdNhanVien { get; set; }
        public string IdPhuCap { get; set; }
        public string NoiDung { get; set; }
    }
}
