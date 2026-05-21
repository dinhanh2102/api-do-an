using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.BangLuong
{
    public class BangLuongSearchResultModel
    {
        public string Id { get; set; }
        public string IdPhongBan { get; set; }
        public string IdDonVi { get; set; }

        public string IdKyCong { get; set; }
        public string IdNhanVien { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }

        public double? LuongCoBan { get; set; }
        public string? TenChucDanh{ get; set; }

        public int? ThangKyCong { get; set; }
        public int NamKyCong { get; set; }
        public double? TongSoCong { get; set; }
        public double? SoNgayCong { get; set; }
        public int? SoNgayMuon { get; set; }
        public int? SoNgayVang { get; set; }

        public string IdPhuCap { get; set; }
        public double? SoTienPhuCap { get; set; }

        public double? SoGioTangCa { get; set; }
        public string IdLoaiCaTangCa { get; set; }
        public int? ThangTangCa { get; set; }
        public int? NamTangCa { get; set; }

        public double? SoTienUngLuong { get; set; }
        public int? ThangUngLuong { get; set; }
        public int? NamUngLuong { get; set; }

        public double? TienLuong { get; set; }
        public double? NgayThuong { get; set; }
        public double? SoTienMuon { get; set; }
        public double? SoTienTangCa { get; set; }
        public string TienLuongThucTeStr { get; set; }
    }

   
}
