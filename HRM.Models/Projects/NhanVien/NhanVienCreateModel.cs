using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.NhanVien
{
    public class NhanVienCreateModel
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public DateTime DOB { get; set; }
        public string CanCuocCongDan { get; set; }
        public string IdChucDanh { get; set; }
        public string SoTKNganHang { get; set; }
        public int? NganHang { get; set; }
        public int? GioiTinh { get; set; }
        public string IdTrinhDo { get; set; }
        public string Avatar { get; set; }
        public string IdBaoHiem { get; set; }
        public bool? IsTruongPhong { get; set; }
    }
}
