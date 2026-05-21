using Microsoft.AspNetCore.Mvc;
using HRM.Common;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Users;
using HRM.Api.Attributes;
using HRM.Models.Cores.User;
using HRM.Services.Cores.Auth;
using HRM.Services.Cores.ConfigInterface;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Cores
{
    [ApiController]
    [ApiHandleExceptionSystem]
    [Route("api/auth")]
    [Logging]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IConfigInterfaceService _configInterfaceService;

        public AuthController(IAuthService authService, IConfigInterfaceService configInterfaceService)
        {
            _authService = authService;
            _configInterfaceService = configInterfaceService;
        }
        /// <summary>
        /// Lấy cấu hình giao diện
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-detail")]
        public async Task<ActionResult<ApiResultModel>> GetConfig()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _configInterfaceService.GetConfigAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="logInModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ActionName(HRMConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel<HRMUserTokenModel>>> LogInAsync([FromBody] HRMLogInModel logInModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _authService.LoginAsync(logInModel, Request);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="RefreshTokenModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh/token")]
        [ActionName(HRMConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel<HRMUserTokenModel>>> RefreshTokenAsync([FromBody] RefreshTokenModel tokenrefresh)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _authService.RefreshToken(CurrentUser.UserId, tokenrefresh.RefreshToken);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
