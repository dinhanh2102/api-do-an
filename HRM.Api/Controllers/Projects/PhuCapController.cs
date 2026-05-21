using HRM.Api.Attributes;
using HRM.Api.Controllers.Cores;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Projects.LoaiPhuCap;
using HRM.Models.Projects.PhuCapNhanVien;
using HRM.Services.Projects.LoaiPhuCap;
using HRM.Services.Projects.PhuCapNhanVien;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Projects
{
    [Route("api/phu-cap")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class PhuCapController : BaseApiController
    {
        private readonly IPhuCapService _phuCapService;
        private readonly IPhuCapNhanVienService _phuCapNhanVienService;

        public PhuCapController(IPhuCapService phuCapService, IPhuCapNhanVienService phuCapNhanVienService)
        {
            _phuCapService = phuCapService;
            _phuCapNhanVienService = phuCapNhanVienService;
        }

        /// <summary>
        /// Tìm kiếm phụ cấp
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-phu-cap")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1591")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<PhuCapSearchModel>>>> SearchPhuCapAsync(PhuCapSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _phuCapService.SearchPhuCapAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới phụ cấp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-phu-cap")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1592")]
        public async Task<ActionResult<ApiResultModel>> CreatePhuCapAsync([FromBody] PhuCapCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _phuCapService.CreatePhuCapAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xoá phụ cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-phu-cap/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F1594")]
        public async Task<ActionResult<ApiResultModel>> DeletePhuCapAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _phuCapService.DeletePhuCapAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy phụ cấp theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-phu-cap-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<PhuCapSearchModel>>> GetPhuCapByIdAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _phuCapService.GetPhuCapByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật phụ cấp
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-phu-cap/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F1593")]
        public async Task<ActionResult<ApiResultModel>> UpdatePhuCapAsync([FromBody] PhuCapCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _phuCapService.UpdatePhuCapAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        ///// <summary>
        ///// Lấy danh sách phụ cấp theo id phụ cấp
        ///// </summary>
        ///// <param name="idPhuCap"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("get-phu-cap")]
        //[ActionName(TextResourceKey.Action_Get)]
        //public async Task<ActionResult<ApiResultModel<PhuCapSearchModel>>> GetComboboxPhuCap([FromQuery] string idPhuCap)
        //{
        //    ApiResultModel apiResultModel = new ApiResultModel();
        //    apiResultModel.Data = await _phuCapService.GetComboboxPhuCap(idPhuCap);
        //    apiResultModel.IsStatus = true;
        //    return Ok(apiResultModel);
        //}

        /// <summary>
        /// Tìm kiếm phụ cấp nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-phu-cap-nhan-vien")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1595")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<PhuCapNhanVienSearchResultModel>>>> SearchPhuCapNhanVienAsync(PhuCapNhanVienSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _phuCapNhanVienService.SearchPhuCapNhanVienAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới phụ cấp nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-phu-cap-nhan-vien")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1596")]
        public async Task<ActionResult<ApiResultModel>> CreatePhuCapNhanVienAsync([FromBody] PhuCapNhanVienCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _phuCapNhanVienService.CreatePhuCapNhanVienAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xoá phụ cấp nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-phu-cap-nhan-vien/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F1598")]
        public async Task<ActionResult<ApiResultModel>> DeletePhuCapNhanVienAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _phuCapNhanVienService.DeletePhuCapNhanVienAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy phụ cấp nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-phu-cap-nhan-vien-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<PhuCapNhanVienSearchModel>>> GetPhuCapNhanVienByIdAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _phuCapNhanVienService.GetPhuCapNhanVienByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật phụ cấp nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-phu-cap-nhan-vien/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F1597")]
        public async Task<ActionResult<ApiResultModel>> UpdatePhuCapNhanVienAsync([FromBody] PhuCapNhanVienCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _phuCapNhanVienService.UpdatePhuCapNhanVienAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
