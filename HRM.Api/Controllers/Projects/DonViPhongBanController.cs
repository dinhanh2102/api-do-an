using HRM.Api.Attributes;
using HRM.Api.Controllers.Cores;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Projects.DonViPhongBan;
using HRM.Services.Projects.DonViPhongBan;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Projects
{
    [Route("api/don-vi-phong-ban")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class DonViPhongBanController : BaseApiController
    {
        private readonly IDonViPhongBanService _donViPhongBanService;

        public DonViPhongBanController(IDonViPhongBanService donViPhongBanService)
        {
            _donViPhongBanService = donViPhongBanService;
        }

        /// <summary>
        /// Tìm kiếm đơn vị
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-don-vi")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1591")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<DonViSearchModel>>>> SearchDonViAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _donViPhongBanService.SearchDonViAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới đơn vị
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-don-vi")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1592")]
        public async Task<ActionResult<ApiResultModel>> CreateDonViAsync([FromBody] DonViCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _donViPhongBanService.CreateDonViAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xoá đơn vị
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-don-vi/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F1594")]
        public async Task<ActionResult<ApiResultModel>> DeleteDonViAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _donViPhongBanService.DeleteDonViAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy đơn vị theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-don-vi-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<DonViSearchModel>>> GetDonViByIdAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _donViPhongBanService.GetDonViByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật đơn vị
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-don-vi/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F1593")]
        public async Task<ActionResult<ApiResultModel>> UpdateDonViAsync([FromBody] DonViCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _donViPhongBanService.UpdateDonViAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách đơn vị theo id đơn vị
        /// </summary>
        /// <param name="idDonVi"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-don-vi")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<DonViSearchModel>>> GetComboboxDonVi([FromQuery] string idDonVi)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _donViPhongBanService.GetComboboxDonVi(idDonVi);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-phong-ban")]
        [ActionName(TextResourceKey.Action_Search)]
        //[AllowPermission(Permissions = "F1595")]
        public async Task<ActionResult<ApiResultModel<SearchBaseResultModel<PhongBanSearchResultModel>>>> SearchPhongBanAsync(PhongBanSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = _donViPhongBanService.SearchPhongBanAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-phong-ban")]
        [ActionName(TextResourceKey.Action_Create)]
        //[AllowPermission(Permissions = "F1596")]
        public async Task<ActionResult<ApiResultModel>> CreatePhongBanAsync([FromBody] PhongBanCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _donViPhongBanService.CreatePhongBanAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xoá phòng ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-phong-ban/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        //[AllowPermission(Permissions = "F1598")]
        public async Task<ActionResult<ApiResultModel>> DeletePhongBanAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _donViPhongBanService.DeletePhongBanAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy phòng ban theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-phong-ban-by-id/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<PhongBanSearchModel>>> GetPhongBanByIdAsync(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _donViPhongBanService.GetPhongBanByIdAsync(id);
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
        [Route("update-phong-ban/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        //[AllowPermission(Permissions = "F1597")]
        public async Task<ActionResult<ApiResultModel>> UpdatePhongBanAsync([FromBody] PhongBanCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _donViPhongBanService.UpdatePhongBanAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

    }
}
