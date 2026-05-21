using HRM.Common.Models;
using HRM.Models.Projects.LoaiPhuCap;
using System.Threading.Tasks;

namespace HRM.Services.Projects.LoaiPhuCap
{
    public interface IPhuCapService
    {
        /// <summary>
        /// Thêm mới phụ cấp
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreatePhuCapAsync(PhuCapCreateModel model, string userId);

        /// <summary>
        /// Xoá phụ cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePhuCapAsync(string id);

        /// <summary>
        /// Lấy phụ cấp theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PhuCapSearchResultModel> GetPhuCapByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm phụ cấp
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<PhuCapSearchResultModel>> SearchPhuCapAsync(PhuCapSearchModel searchModel);

        /// <summary>
        /// Cập nhật phụ cấp
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdatePhuCapAsync(string id, PhuCapCreateModel model, string userId);
    }
}
