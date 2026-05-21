using HRM.Api.Attributes;
using HRM.Services.Cores.UserHistorys;
using Microsoft.AspNetCore.Mvc;
using HRM.Common;
using HRM.Common.Attributes;
using HRM.Common.Helpers;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Common.Models;
using System.Threading.Tasks;
using HRM.Models.Cores.UserHistory;

namespace HRM.Api.Controllers.Cores
{
    [Route("api/user-history")]
    [ApiController]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    [Logging]
    [HRMAuthorize]
    public class UserHistoryController : BaseApiController
    {
        private readonly IUserHistoryService _userHistoryService;
        public UserHistoryController(IUserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        /// <summary>
        /// Tìm kiếm lịch sử
        /// </summary>
        /// <param name="modelSearch">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0151")]
        public async Task<ActionResult<SearchBaseResultModel<UserHistorySearchResultModel>>> SearchHistory([FromBody] UserHistorySearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await _userHistoryService.SearchHistoryAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model">Model điều kiện xuất</param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-excel")]
        [ActionName(TextResourceKey.Action_Export_Excel)]
        [AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportExcel([FromBody] UserHistorySearchModel searchModel)
        {
            var file = await _userHistoryService.ExportFileAsync(searchModel, HRMConstants.TemplateHistory, HRMConstants.OptionExport.Excel);

            return File(file.ToArray(), FileHelper.GetContentType(".xlsx"), "LichSuThaoTac.xlsx");
        }

        /// <summary>
        /// Xuất pdf
        /// </summary>
        /// <param name="model">Model điều kiện xuất</param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-pdf")]
        [ActionName(TextResourceKey.Action_Export_Pdf)]
        [AllowPermission(Permissions = "F0151")]
        public async Task<IActionResult> ExportPDF([FromBody] UserHistorySearchModel searchModel)
        {
            var file = await _userHistoryService.ExportFileAsync(searchModel, HRMConstants.TemplateHistory, HRMConstants.OptionExport.Pdf);
            return File(file.ToArray(), FileHelper.GetContentType(".pdf"), "LichSuThaoTac.pdf");
        }
    }
}
