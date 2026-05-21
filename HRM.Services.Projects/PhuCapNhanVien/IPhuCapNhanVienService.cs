using HRM.Common.Models;
using HRM.Models.Projects.PhuCapNhanVien;
using System.Threading.Tasks;

namespace HRM.Services.Projects.PhuCapNhanVien
{
    public interface IPhuCapNhanVienService
    {
        /// <summary>
        /// Thêm mới phụ cấp nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreatePhuCapNhanVienAsync(PhuCapNhanVienCreateModel model, string userId);

        /// <summary>
        /// Xoá phụ cấp nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePhuCapNhanVienAsync(string id);

        /// <summary>
        /// Lấy phụ cấp nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PhuCapNhanVienSearchResultModel> GetPhuCapNhanVienByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm phụ cấp nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<PhuCapNhanVienSearchResultModel>> SearchPhuCapNhanVienAsync(PhuCapNhanVienSearchModel searchModel);

        /// <summary>
        /// Cập nhật phụ cấp nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdatePhuCapNhanVienAsync(string id, PhuCapNhanVienCreateModel model, string userId);
    }
}
