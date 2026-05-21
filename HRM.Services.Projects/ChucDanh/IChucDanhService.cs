using HRM.Common.Models;
using HRM.Models.Projects.ChucDanh;
using System.Threading.Tasks;

namespace HRM.Services.Projects.ChucDanh
{
    public interface IChucDanhService
    {

        /// <summary>
        /// Thêm mới chức danh
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateChucDanhAsync(ChucDanhCreateModel model, string userId);

        /// <summary>
        /// Xoá chức danh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteChucDanhAsync(string id);

        /// <summary>
        /// Lấy chức danh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ChucDanhSearchResultModel> GetChucDanhByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm chức danh
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<ChucDanhSearchResultModel>> SearchChucDanhAsync(ChucDanhSearchModel searchModel);

        /// <summary>
        /// Cập nhật chức danh
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateChucDanhAsync(string id, ChucDanhCreateModel model, string userId);
    }
}
