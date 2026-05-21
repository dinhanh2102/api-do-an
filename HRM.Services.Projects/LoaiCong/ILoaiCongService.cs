using HRM.Models.Projects.LoaiCong;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Projects.LoaiCong
{
    public interface ILoaiCongService
    {
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateLoaiCongAsync(LoaiCongCreateModel model, string userId);

        /// <summary>
        /// Xoá nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteLoaiCongAsync(string id, string userId);

        /// <summary>
        /// Lấy nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LoaiCongSearchResultModel> GetLoaiCongByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<LoaiCongSearchResultModel>> SearchLoaiCongAsync(LoaiCongSearchModel searchModel);

        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateLoaiCongAsync(string id, LoaiCongCreateModel model, string userId);
    }
}
