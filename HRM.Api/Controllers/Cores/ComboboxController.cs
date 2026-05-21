using HRM.Services.Cores.Combobox;
using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using System;
using System.Threading.Tasks;
using HRM.Common.Resource;
using HRM.Models.Projects.NhanVien;

namespace HRM.Api.Controllers.Cores
{
    [Route("api/combobox")]
    [ApiController]
    //[HRMAuthorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class ComboboxController : BaseApiController
    {
        private readonly IComboboxService comboboxService;

        public ComboboxController(IComboboxService comboboxService)
        {
            this.comboboxService = comboboxService;
        }

        /// <summary>
        /// Danh sách nhóm danh mục
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-group-user")]
        public async Task<ActionResult<ApiResultModel>> GetAllGroupUser()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetAllGroupUser();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-user")]
        public async Task<ActionResult<ApiResultModel>> GetAllUser()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetAllUser();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }


        /// <summary>
        /// Danh sách menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-menu")]
        public async Task<ActionResult<ApiResultModel>> GetMenu()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetMenu();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Hàm lấy giờ server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-date-now-server")]
        public async Task<ActionResult> GetDateNowServer()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = DateTime.Now;
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-nhan-vien")]
        public async Task<ActionResult<ApiResultModel>> GetNhanVien()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetNhanVien();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-don-vi")]
        public async Task<ActionResult<ApiResultModel>> GetDonVi()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetDonVi();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách phòng ban theo đơn vị
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-phong-ban-by-idDonVi")]
        public async Task<ActionResult<ApiResultModel>> GetPhongBanByIdDonVi([FromQuery] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetPhongBanByIdDonVi(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách chức danh theo phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-chuc-danh-by-id-phong-ban/{id}")]
        public async Task<ActionResult<ApiResultModel>> GetChucDanhByIdPhongBan(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetChucDanhByIdPhongBan(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy ra danh sách ngân hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-ngan-hang")]
        public async Task<ActionResult<ApiResultModel>> GetNganHang()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetNganHang();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy ra danh sách trình độ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-trinh-do")]
        public async Task<ActionResult<ApiResultModel>> GetTrinhDo()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetTrinhDo();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy ra danh sách phụ cấp
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-phu-cap")]
        public async Task<ActionResult<ApiResultModel>> GetPhuCap()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetPhuCap();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-nhan-vien-by-code/{code}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<NhanVienSearchModel>>> GetNhanVienByCodeAsync(string code)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetNhanVienByCodeAsync(code);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// Lấy nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-group-user/{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel>> GetGroupUser(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetGroupUser(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
