using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Models.Cores.ConfigInterface;
using HRM.Services.Cores.ConfigInterface;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Cores
{

    [Route("api/config-interface")]
    [ApiController]
    [HRMAuthorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class ConfigInterfaceController : BaseApiController
    {
        private readonly IConfigInterfaceService _configInterfaceService;
        public ConfigInterfaceController(IConfigInterfaceService configInterfaceService)
        {
            _configInterfaceService = configInterfaceService;
        }

        /// <summary>
        /// Thêm cấu hình giao diện
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-or-update")]
        [AllowPermission(Permissions = "F0161")]
        public async Task<ActionResult<ApiResultModel>> CreateOrUpdate(ConfigInterfaceModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _configInterfaceService.CreateOrUpdateAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Chi tiết cấu hình giao diện
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-detail")]
        [AllowPermission(Permissions = "F0162")]
        public async Task<ActionResult<ApiResultModel>> GetConfig()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _configInterfaceService.GetConfigAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
