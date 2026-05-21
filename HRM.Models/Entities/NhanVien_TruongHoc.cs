using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class NhanVien_TruongHoc
    {
        public string Id { get; set; }
        public string IdNhanVien { get; set; }
        public string IdChuyenNganh { get; set; }
        public int LoaiTotNghiep { get; set; }
        public int NamTotNghiep { get; set; }
        public string MoTa { get; set; }
    }
}
