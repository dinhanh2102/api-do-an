using HRM.Models.Projects.LoaiCa;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Projects.LoaiCa
{
    public interface ILoaiCaService
    {
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateLoaiCaAsync(LoaiCaCreateModel model, string userId);

        /// <summary>
        /// Xoá nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteLoaiCaAsync(string id, string userId);

        /// <summary>
        /// Lấy nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LoaiCaSearchResultModel> GetLoaiCaByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<LoaiCaSearchResultModel>> SearchLoaiCaAsync(LoaiCaSearchModel searchModel);

        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateLoaiCaAsync(string id, LoaiCaCreateModel model, string userId);
    }
}
