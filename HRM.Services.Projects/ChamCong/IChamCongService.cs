using HRM.Models.Projects.ChamCong;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Models.Entities;
using HRM.Models.Projects.NhanVien;

namespace HRM.Services.Projects.ChamCong
{
    public interface IChamCongService
    {
        Task ChamCong(ChamCongCreateModel model);
        Task createChamCong(ChamCongCreateHandle model);
        Task<ChamCongGetModel> getChamCongDetail(string id);
        Task<NhanVienSearchResultModel> getNhanVienTheoMaNhanVien(string maNhanVien);
        Task LayDanhSachChamCongTheoThang(BangCongGetModel model);
        Task updateChamCong(string id, ChamCongGetModel model);
    }
}
