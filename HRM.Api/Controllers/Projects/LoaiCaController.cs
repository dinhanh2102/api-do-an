using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Api.Attributes;
using HRM.Common.Models;
using System.Threading.Tasks;
using HRM.Api.Controllers.Cores;
using HRM.Services.Projects.LoaiCa;
using HRM.Models.Projects.LoaiCa;

namespace HRM.Api.Controllers.LoaiCa
{
    [Route("api/loai-ca")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class LoaiCaController : BaseApiController
    {
        private readonly ILoaiCaService loaiCa;
        public LoaiCaController(ILoaiCaService loaiCaService)
        {
            loaiCa = loaiCaService;
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
        public async Task<ActionResult<SearchBaseResultModel<LoaiCaSearchResultModel>>> SearchLoaiCa([FromBody] LoaiCaSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await loaiCa.SearchLoaiCaAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin loại ca theo Id
        /// </summary>
        /// <param name="id">Id loại ca</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-loai-ca-by-id")]
        [ActionName(TextResourceKey.Action_Get)]
        //[AllowPermission(Permissions = "F0114;F0112")]
        public async Task<ActionResult<LoaiCaSearchModel>> GetLoaiCaById([FromQuery] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await loaiCa.GetLoaiCaByIdAsync(id);
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
        public async Task<ActionResult<ApiResultModel>> CreateLoaiCa([FromBody] LoaiCaCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await loaiCa.CreateLoaiCaAsync(model, CurrentUser.UserId);
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
        public async Task<ActionResult<ApiResultModel>> UpdateLoaiCa([FromRoute] string id, [FromBody] LoaiCaCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await loaiCa.UpdateLoaiCaAsync(id, model, CurrentUser.UserId);
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
        public async Task<ActionResult<ApiResultModel>> DeleteLoaiCa([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await loaiCa.DeleteLoaiCaAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}