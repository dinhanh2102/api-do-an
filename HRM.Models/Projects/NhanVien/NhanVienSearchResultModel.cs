using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.NhanVien
{
    public class NhanVienSearchResultModel
    {
        public string Id { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UserId { get; set; }
        public DateTime DOB { get; set; }
        public string CanCuocCongDan { get; set; }
        public string IdDonVi { get; set; }
        public string IdPhongBan { get; set; }
        public string IdChucDanh { get; set; }
        public string TenChucDanh { get; set; }
        public string SoTKNganHang { get; set; }
        public int? NganHang { get; set; }
        public string TenNganHang { get; set; }
        public int? GioiTinh { get; set; }
        public string IdTrinhDo { get; set; }
        public string TenTrinhDo { get; set; }
        public string IdBaoHiem { get; set; }
        public string TenBaoHiem { get; set; }
        public string Avatar { get; set; }
        public bool? IsTruongPhong { get; set; }
        public string TenPhongBan { get; set; }
        public string TenDonVi { get; set; }
    }
}
