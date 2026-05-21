using HRM.Common;
using HRM.Common.Models;
using System.IO;
using System.Threading.Tasks;
using HRM.Models.Cores.UserHistory;

namespace HRM.Services.Cores.UserHistorys
{
    public interface IUserHistoryService
    {
        /// <summary>
        /// Tìm kiếm lịch sử thao tác người dùng
        /// </summary>
        /// <param name="searchModel">Điều kiện tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<UserHistorySearchResultModel>> SearchHistoryAsync(UserHistorySearchModel searchModel);

        /// <summary>
        /// Xuất danh sách lịch sử thao tác
        /// </summary>
        /// <param name="searchModel">Thông tin báo cáo</param>
        /// <returns></returns>
        Task<MemoryStream> ExportFileAsync(UserHistorySearchModel searchModel, string pathTemplate, HRMConstants.OptionExport optionExport);
    }
}
