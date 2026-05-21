using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Api.Attributes;
using HRM.Common.Models;
using System.Threading.Tasks;
using HRM.Api.Controllers.Cores;
using HRM.Services.Projects.KyCong;
using HRM.Models.Projects.KyCong;
using HRM.Models.Entities;

namespace HRM.Api.Controllers.KyCong
{
    [Route("api/ky-cong")]
    [ApiController]
    [ValidateModel]
    [Logging]
    //[HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class KyCongController : BaseApiController
    {
        private readonly IKyCongService kyCong;
        public KyCongController(IKyCongService kyCongService)
        {
            kyCong = kyCongService;
        }

        /// <summary>
        /// Tìm kiếm kỳ công
        /// </summary>
        /// <param name="modelSearch">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F0114")]
        public async Task<ActionResult<SearchBaseResultModel<KyCongSearchResultModel>>> SearchKyCong([FromBody] KyCongSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await kyCong.SearchKyCongAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin kỳ công theo Id
        /// </summary>
        /// <param name="id">Id kỳ công</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-ky-cong-by-id")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0114;F0112")]
        public async Task<ActionResult<KyCongSearchModel>> GetKyCongById([FromQuery] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await kyCong.GetKyCongByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới kỳ công
        /// </summary>
        /// <param name="model">Model thông tin thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0111")]
        public async Task<ActionResult<ApiResultModel>> CreateKyCong([FromBody] KyCongCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await kyCong.CreateKyCongAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập kỳ công
        /// </summary>
        /// <param name="id">Id kỳ công</param>
        /// <param name="model">Model thông tin cập nhật</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        public async Task<ActionResult<ApiResultModel>> UpdateKyCong([FromRoute] string id, [FromBody] KyCongCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await kyCong.UpdateKyCongAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa kỳ công
        /// </summary>
        /// <param name="id">Id kỳ công</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> DeleteKyCong([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await kyCong.DeleteKyCongAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Phát sinh kỳ công
        /// </summary>
        /// <param name="id">Id kỳ công</param>
        /// <returns></returns>
        [HttpPost]
        [Route("phat-sinh-ky-cong")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> PhatSinhKyCong([FromBody] PhatSinhKyCongCreate model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await kyCong.PhatSinhKyCongChiTiet(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy danh sách kỳ công chi tiết
        /// </summary>
        /// <param name="id">Id kỳ công</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-ky-cong-chi-tiet/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> GetListKyCongChiTiet(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await kyCong.getListKyCongChiTiet(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin kỳ công theo Id
        /// </summary>
        /// <param name="id">Id kỳ công</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-chi-tiet-ky-cong/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0114;F0112")]
        public async Task<ActionResult<KyCongSearchModel>> GetChiTietKyCong([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await kyCong.getListChamCong(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa chấm công
        /// </summary>
        /// <param name="id">Id Chấm công</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("xoa-cham-cong/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> DeleteChamCong([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await kyCong.DeleteChamCongAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}