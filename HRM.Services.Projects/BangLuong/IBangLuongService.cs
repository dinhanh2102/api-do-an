using HRM.Common.Models;
using HRM.Models.Projects.BangLuong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Projects.BangLuong
{
    public interface IBangLuongService
    {
        /// <summary>
        /// Thêm mới bảng lương
        /// </summary>
        /// <returns></returns>
        Task CreateBangLuongAsync(BangLuongCreateModel model, string userId);

        /// <summary>
        /// Tìm kiếm bảng lương
        /// </summary>
        /// <returns></returns>
        Task<SearchBaseResultModel<BangLuongSearchResultModel>> SearchBangLuongAsync(BangLuongSearchModel searchModel, string userId);
    }
}
