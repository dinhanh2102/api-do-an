using HRM.Api.Attributes;
using HRM.Api.Controllers.Cores;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Projects.LoaiCong;
using HRM.Services.Projects.LoaiCong;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.LoaiCong
{
    [Route("api/loai-cong")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class LoaiCongController : BaseApiController
    {
        private readonly ILoaiCongService LoaiCong;
        public LoaiCongController(ILoaiCongService LoaiCongService)
        {
            LoaiCong = LoaiCongService;
        }

        /// <summary>
        /// Tìm kiếm loại ca
        /// </summary>
        /// <param name="modelSearch">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0114")]
        public async Task<ActionResult<SearchBaseResultModel<LoaiCongSearchResultModel>>> SearchLoaiCong([FromBody] LoaiCongSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await LoaiCong.SearchLoaiCongAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin loại ca theo Id
        /// </summary>
        /// <param name="id">Id loại ca</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-loai-cong-by-id")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0114;F0112")]
        public async Task<ActionResult<LoaiCongSearchModel>> GetLoaiCongById([FromQuery] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await LoaiCong.GetLoaiCongByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới loại ca
        /// </summary>
        /// <param name="model">Model thông tin thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0111")]
        public async Task<ActionResult<ApiResultModel>> CreateLoaiCong([FromBody] LoaiCongCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await LoaiCong.CreateLoaiCongAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập loại ca
        /// </summary>
        /// <param name="id">Id loại ca</param>
        /// <param name="model">Model thông tin cập nhật</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        public async Task<ActionResult<ApiResultModel>> UpdateLoaiCong([FromRoute] string id, [FromBody] LoaiCongCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await LoaiCong.UpdateLoaiCongAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa loại ca
        /// </summary>
        /// <param name="id">Id loại ca</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> DeleteLoaiCong([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await LoaiCong.DeleteLoaiCongAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}