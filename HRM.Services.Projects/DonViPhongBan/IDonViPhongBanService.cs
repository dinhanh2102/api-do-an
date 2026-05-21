using HRM.Common.Models;
using HRM.Models.Projects.DonViPhongBan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Services.Projects.DonViPhongBan
{
    public interface IDonViPhongBanService
    {

        /// <summary>
        /// Tạo mới đơn vị
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateDonViAsync(DonViCreateModel model, string userId);

        /// <summary>
        /// Thêm mới phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreatePhongBanAsync(PhongBanCreateModel model, string userId);

        /// <summary>
        /// Xoá đơn vị
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteDonViAsync(string id);

        /// <summary>
        /// Xoá phòng ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePhongBanAsync(string id);

        /// <summary>
        /// Lấy danh sách đơn vị theo id đơn vị
        /// </summary>
        /// <param name="idDonVi"></param>
        /// <returns></returns>
        Task<List<DonViSearchModel>> GetComboboxDonVi(string idDonVi);

        /// <summary>
        /// Lấy đơn vị theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DonViSearchModel> GetDonViByIdAsync(string id);

        /// <summary>
        /// Lấy phòng ban theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PhongBanSearchResultModel> GetPhongBanByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm đơn vị
        /// </summary>
        /// <returns></returns>
        Task<SearchBaseResultModel<DonViSearchModel>> SearchDonViAsync();

        /// <summary>
        /// Tìm kiếm phòng ban
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<PhongBanSearchResultModel>> SearchPhongBanAsync(PhongBanSearchModel searchModel);

        /// <summary>
        /// Cập nhật đơn vị
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateDonViAsync(string id, DonViCreateModel model, string userId);

        /// <summary>
        /// Cập nhật phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdatePhongBanAsync(string id, PhongBanCreateModel model, string userId);
    }
}
