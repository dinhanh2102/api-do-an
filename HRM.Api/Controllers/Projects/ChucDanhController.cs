using HRM.Api.Attributes;
using HRM.Api.Controllers.Cores;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Projects.ChucDanh;
using HRM.Services.Projects.ChucDanh;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Projects
{
    [Route("api/chuc-danh")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class ChucDanhController : BaseApiController
    {
        private readonly IChucDanhService _chucDanhService;

        public ChucDanhController(IChucDanhService chucDanhService)
        {
            _chucDanhService = chucDanhService;
        }

        /// <summary>
        /// Tìm kiếm chức danh
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-chuc-danh")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1011")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<ChucDanhSearchResultModel>>>> SearchChucDanhAsync(ChucDanhSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _chucDanhService.SearchChucDanhAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới chức danh
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-chuc-danh")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1012")]
        public async Task<ActionResult<ApiResultModel>> CreateChucDanhAsync([FromBody] ChucDanhCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _chucDanhService.CreateChucDanhAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xoá chức danh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-chuc-danh/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F1014")]
        public async Task<ActionResult<ApiResultModel>> DeleteChucDanhAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _chucDanhService.DeleteChucDanhAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy chức danh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-chuc-danh-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<ChucDanhSearchModel>>> GetChucDanhByIdAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _chucDanhService.GetChucDanhByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật chức danh
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-chuc-danh/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F1013")]
        public async Task<ActionResult<ApiResultModel>> UpdateChucDanhAsync([FromBody] ChucDanhCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _chucDanhService.UpdateChucDanhAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
