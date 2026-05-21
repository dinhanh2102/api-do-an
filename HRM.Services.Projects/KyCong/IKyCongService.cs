using HRM.Models.Projects.KyCong;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Models.Entities;

namespace HRM.Services.Projects.KyCong
{
    public interface IKyCongService
    {
        /// <summary>
        /// Thêm mới kỳ công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateKyCongAsync(KyCongCreateModel model);
        Task DeleteChamCongAsync(string id);

        /// <summary>
        /// Xoá kỳ công
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteKyCongAsync(string id, string userId);

        /// <summary>
        /// Lấy kỳ công theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<KyCongSearchResultModel> GetKyCongByIdAsync(string id);
        Task<ModelCongChiTietResult> getListChamCong(string idKyCong, string userId);

        /// <summary>
        /// Lấy danh sách kỳ công chi tiết
        /// </summary>
        /// <param name="idKyCong"></param>
        /// <returns></returns>
        Task<KyCongResultModel> getListKyCongChiTiet(string idKyCong);

        /// <summary>
        /// Phát sinh kỳ công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task PhatSinhKyCongChiTiet(PhatSinhKyCongCreate model);

        /// <summary>
        /// Tìm kiếm kỳ công
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<KyCongSearchResultModel>> SearchKyCongAsync(KyCongSearchModel searchModel);

        /// <summary>
        /// Cập nhật kỳ công
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateKyCongAsync(string id, KyCongCreateModel model, string userId);
    }
}
