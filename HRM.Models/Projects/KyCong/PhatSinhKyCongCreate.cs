using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.KyCong
{
    public class PhatSinhKyCongCreate
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string IdKyCong { get; set; }
    }
    public class KyCongResultModel
    {
        public List<KyCongChiTietModel> ChiTiets = new List<KyCongChiTietModel>();
        public List<NgayHeader> ListHeader = new List<NgayHeader>();
        public string TieuDeKyCongChiTiet { get; set; }
    }
    public class KyCongChiTietModel
    {
        public string Id { get; set; }
        public string IdKyCong { get; set; }
        public string IdNhanVien { get; set; }
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string MaCongTy { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string D5 { get; set; }
        public string D6 { get; set; }
        public string D7 { get; set; }
        public string D8 { get; set; }
        public string D9 { get; set; }
        public string D10 { get; set; }
        public string D11 { get; set; }
        public string D12 { get; set; }
        public string D13 { get; set; }
        public string D14 { get; set; }
        public string D15 { get; set; }
        public string D16 { get; set; }
        public string D17 { get; set; }
        public string D18 { get; set; }
        public string D19 { get; set; }
        public string D20 { get; set; }
        public string D21 { get; set; }
        public string D22 { get; set; }
        public string D23 { get; set; }
        public string D24 { get; set; }
        public string D25 { get; set; }
        public string D26 { get; set; }
        public string D27 { get; set; }
        public string D28 { get; set; }
        public string D29 { get; set; }
        public string D30 { get; set; }
        public string D31 { get; set; }
        public double? NgayCong { get; set; }
        public double? NgayPhep { get; set; }
        public double? NghiKhongPhep { get; set; }
        public double? CongNgayLe { get; set; }
        public double? CongChuNhat { get; set; }
        public double? TongNgayCong { get; set; }
        public int? SoNgayDiMuon { get; set; }
    }

    public class NgayHeader
    {
        public int Ngay { get; set; }
        public string Thu { get; set; }
    }
    public class NgayChamCong
    {
        public string IdChamCong { get; set; }
        public int? Ngay { get; set; }
        public DateTime? ThoiGianVao { get; set; }
        public DateTime? ThoiGianRa { get; set; }
        public string Label { get; set; }
    }
    public class ModelCongChiTietResult
    {
        public List<CongChiTietResult> CongChiTietResults = new List<CongChiTietResult>();
        public string Title { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoNgayTrongThang { get; set; }
        public List<NgayHeader> ngayHeaders = new List<NgayHeader>();

    }
    public class CongChiTietResult
    {
        public List<NgayChamCong> ChamCongList = new List<NgayChamCong>();
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public double SoNgayCong { get; set; }
        public double SoNgayMuon { get; set; }
    }

    public class NhanVienKyCongDetailModel
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string IdNhanVien { get; set; }
        public string IdKyCong { get; set; }
        public string IdBangCong { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public DateTime? ThoiGianRa { get; set; }
        public DateTime? ThoiGianVao { get; set; }
        public double SoNgayCong { get; set; }
        public double SoCong { get; set; }
    }

}

