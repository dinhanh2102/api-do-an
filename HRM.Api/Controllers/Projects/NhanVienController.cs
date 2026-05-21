using HRM.Api.Attributes;
using HRM.Api.Controllers.Cores;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Projects.NhanVien;
using HRM.Services.Projects.NhanVien;
using HRM.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Projects
{
    [Route("api/nhan-vien")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class NhanVienController : BaseApiController
    {
        private readonly INhanVienService _nhanVienService;

        public NhanVienController(INhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        /// <summary>
        /// Tìm kiếm phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-nhan-vien")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1566")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<NhanVienSearchResultModel>>>> SearchNhanVienAsync(NhanVienSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _nhanVienService.SearchNhanVienAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-nhan-vien")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1567")]
        public async Task<ActionResult<ApiResultModel>> CreateNhanVienAsync([FromBody] NhanVienCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _nhanVienService.CreateNhanVienAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xoá phòng ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-nhan-vien/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F1569")]
        public async Task<ActionResult<ApiResultModel>> DeleteNhanVienAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _nhanVienService.DeleteNhanVienAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy phòng ban theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-nhan-vien-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<NhanVienSearchModel>>> GetNhanVienByIdAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _nhanVienService.GetNhanVienByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-nhan-vien/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F1568")]
        public async Task<ActionResult<ApiResultModel>> UpdateNhanVienAsync([FromBody] NhanVienCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _nhanVienService.UpdateNhanVienAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }



    }
}
