using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class NhanVien_GiaDinh
    {
        public string Id { get; set; }
        public string IdNhanVien { get; set; }
        public string MoiQuanHe { get; set; }
        public string HoVaTen { get; set; }
        public string NamSinh { get; set; }
        public string QueQuan { get; set; }
        public string NgheNghiep { get; set; }
        public bool IsGDBenVoChong { get; set; }
    }
}
