using HRM.Models.Cores.Combobox;
using HRM.Models.Projects.NhanVien;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Services.Cores.Combobox
{
    public interface IComboboxService
    {
        /// <summary>
        /// Danh sách giới tính
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllGioiTinh();

        /// <summary>
        /// Lấy danh sách nhóm người dùng
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllGroupUser();

        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllUser();

        /// <summary>
        /// Lấy danh sách bảo hiểm
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetBaoHiem();

        /// <summary>
        /// lấy ra danh sách chức danh theo phòng ban
        /// </summary>
        /// <returns></returns> 
        Task<List<ComboboxModel>> GetChucDanhByIdPhongBan(string idPhongBan);

        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetDonVi();
        /// <summary>
        /// Lấy người dùng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetGroupUser(string id);

        /// <summary>
        /// Lấy danh sách menu
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetMenu();

        /// <summary>
        /// Lấy danh sách ngân hàng 
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetNganHang();

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetNhanVien();
        Task<NhanVienSearchResultModel> GetNhanVienByCodeAsync(string code);

        /// <summary>
        /// Lấy danh sách phòng ban theo id đơn vị
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetPhongBanByIdDonVi(string idDonVi);

        /// <summary>
        /// Danh sách phụ cấp
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetPhuCap();

        /// <summary>
        /// Danh sách trình độ
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetTrinhDo();
    }
}
