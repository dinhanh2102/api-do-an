using HRM.Api.Attributes;
using HRM.Api.Controllers.Cores;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Projects.BangLuong;
using HRM.Services.Projects.BangLuong;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Projects
{
    [Route("api/bang-luong")]
    //[ApiController]
    //[ValidateModel]
    //[Logging]
    //[HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class BangLuongController : BaseApiController
    {
        private readonly IBangLuongService _bangLuongService;

        public BangLuongController(IBangLuongService bangLuongService)
        {
            _bangLuongService = bangLuongService;
        }

        /// <summary>
        /// Tìm kiếm bảng lương
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1591")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<BangLuongSearchModel>>>> SearchBangLuongAsync(BangLuongSearchModel model)
        {   
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _bangLuongService.SearchBangLuongAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới bảng lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1592")]
        public async Task<ActionResult<ApiResultModel>> CreateBangLuongAsync([FromBody] BangLuongCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _bangLuongService.CreateBangLuongAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
