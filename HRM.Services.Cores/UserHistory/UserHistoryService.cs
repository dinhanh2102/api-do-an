using HRM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using HRM.Common;
using HRM.Document.Excel;
using HRM.Models.Cores.UserHistory;
using HRM.Common.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;

namespace HRM.Services.Cores.UserHistorys
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly IExcelService _excelService;

        public UserHistoryService(CoreProjectContext sqlContext, IExcelService excelService)
        {
            this._sqlContext = sqlContext;
            this._excelService = excelService;
        }

        /// <summary>
        /// Tìm kiếm lịch sử thao tác người dùng
        /// </summary>
        /// <param name="searchModel">Điều kiện tìm kiếm</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<UserHistorySearchResultModel>> SearchHistoryAsync(UserHistorySearchModel searchModel)
        {
            //SearchBaseResultModel<UserHistorySearchResultModel> resultModel = new SearchBaseResultModel<UserHistorySearchResultModel>();

            //var dataQuery = (from a in _sqlContext.UserHistory.AsNoTracking()
            //                 join b in _sqlContext.User.AsNoTracking() on a.UserId equals b.Id into ab
            //                 from ba in ab.DefaultIfEmpty()
            //                 orderby a.CreateDate descending
            //                 select new UserHistorySearchResultModel
            //                 {
            //                     BrowserName = a.BrowserName,
            //                     BrowserVersion = a.BrowserVersion,
            //                     ClientIP = a.ClientIP,
            //                     CreateDate = a.CreateDate,
            //                     Device = a.Device,
            //                     OS = a.OS,
            //                     UserName = ba != null ? ba.UserName : "",
            //                     Name = a.Name,
            //                     UserId = ba != null ? ba.Id : "",
            //                     Type = a.Type,
            //                     FullName = ba.FullName
            //                 }).AsQueryable();

            //if (searchModel.Type.HasValue)
            //{
            //    dataQuery = dataQuery.Where(a => a.Type == searchModel.Type);
            //}

            //if (!string.IsNullOrEmpty(searchModel.Name))
            //{
            //    dataQuery = dataQuery.Where(a => a.Name.ToLower().Contains(searchModel.Name.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(searchModel.UserId))
            //{
            //    dataQuery = dataQuery.Where(a => a.UserId.Equals(searchModel.UserId));
            //}

            //if (searchModel.DateFrom.HasValue)
            //{
            //    dataQuery = dataQuery.Where(a => a.CreateDate >= DateTimeHelper.ToStartDate(searchModel.DateFrom.Value));
            //}

            //if (searchModel.DateTo.HasValue)
            //{
            //    dataQuery = dataQuery.Where(a => a.CreateDate <= DateTimeHelper.ToEndDate(searchModel.DateTo.Value));
            //}
            //if (!string.IsNullOrEmpty(searchModel.OrderBy))
            //{
            //    dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            //}

            //resultModel.TotalItems = dataQuery.Count();
            //resultModel.DataResults = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            //return resultModel;
            return null;
        }

        /// <summary>
        /// Xuất danh sách lịch sử thao tác
        /// </summary>
        /// <param name="searchModel">Điều kiện lọc</param>
        /// <param name="pathTemplate">ĐƯờng dẫn file mẫu</param>
        /// <param name="optionExport">Loại file xuất ra (xlsx,pdf)</param>
        /// <returns></returns>
        public async Task<MemoryStream> ExportFileAsync(UserHistorySearchModel searchModel, string pathTemplate, HRMConstants.OptionExport optionExport)
        {
            SearchBaseResultModel<UserHistorySearchResultModel> searchResult = SearchHistoryAsync(searchModel).Result;

            int index = 1;
            var dataExport = searchResult.DataResults.Select(a => new
            {
                Index = index++,
                Type = a.Type == HRMConstants.UserHistory_Type_Login ? "Đăng nhập" : a.Type == HRMConstants.UserHistory_Type_Data ? "Khai thác dữ liệu" : "",
                a.FullName,
                a.UserName,
                a.BrowserName,
                a.BrowserVersion,
                a.ClientIP,
                a.Device,
                a.OS,
                ThoiGian = a.CreateDate.ToStringDDMMYY(),
                a.Name
            }).ToList();

            MemoryStream streamFile = null;
            if (optionExport == HRMConstants.OptionExport.Excel)
            {
                streamFile = _excelService.ExportExcel(dataExport, pathTemplate, 11);
            }
            else if (optionExport == HRMConstants.OptionExport.Pdf)
            {
                streamFile = _excelService.ExportPdf(dataExport, pathTemplate, 11);
            }

            return streamFile;
        }
    }
}
