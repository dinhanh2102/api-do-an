using Microsoft.AspNetCore.Mvc;
using HRM.Common;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Api.Attributes;
using HRM.Models.Cores.GroupFunction;
using HRM.Models.Cores.User;
using HRM.Services.Cores.Auth;
using HRM.Services.Cores.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Cores
{
    [Route("api/users")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Tìm kiếm tài khoản
        /// </summary>
        /// <param name="searchModel">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0104")]
        public async Task<ActionResult<SearchBaseResultModel<UserSearchResultModel>>> SearchUser([FromBody] UserSearchModel searchModel)
        {
            searchModel.UserId = CurrentUser.UserId;
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _userService.SearchUserAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới tài khoản
        /// </summary>
        /// <param name="model">Model thông tin tài khoản</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0101")]
        public async Task<ActionResult<ApiResultModel>> CreateUser([FromBody] UserCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.CreateUserAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id">Id tài khoản</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0102")]
        public async Task<ActionResult<ApiResultModel>> UpdateUser([FromRoute] string id, [FromBody] UserCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.UpdateUserAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0103")]
        public async Task<ActionResult<ApiResultModel>> DeleteUser(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.DeleteUserAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin tài khoản theo id
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-by-id/{userId}")]
        [ActionName(TextResourceKey.Action_Get)]
        [AllowPermission(Permissions = "F0104;F0102")]
        public async Task<ActionResult<ApiResultModel<UserCreateModel>>> GetUserById([FromRoute] string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _userService.GetUserByIdAsnyc(userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy quyền quyền theo id nhóm quyền
        /// </summary>
        /// <param name="groupUserId">Id nhóm quyền</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-permission")]
        [AllowPermission(Permissions = "F0101;F0102")]
        [ActionName(HRMConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel<List<GroupFunctionModel>>>> GetPermission([FromQuery] string groupUserId)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _userService.GetPermissionAsync(groupUserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Mở/Khóa tài khoản
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <param name="isunlock">Trạng thái</param>
        /// <returns></returns>
        [HttpPut]
        [Route("lock/{userId}")]
        [ActionName(TextResourceKey.Action_User_Lock)]
        [AllowPermission(Permissions = "F0106")]
        public async Task<ActionResult<ApiResultModel>> UserLock(string userId, [FromQuery] bool isunlock)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.UserLockAsync(userId, isunlock);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đổi mật khẩu cho tài khoảng
        /// </summary>
        /// <param name="model">Model thông tin thay đổi</param>
        /// <returns></returns>
        [HttpPut]
        [Route("change-pass")]
        [ActionName(TextResourceKey.Action_User_ChangePass)]
        [AllowPermission(Permissions = "F0105")]
        public async Task<ActionResult<ApiResultModel>> ResetPassword([FromBody] ChangePasswordModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            model.Id = CurrentUser.UserId;
            await _userService.ChangePasswordAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="model">Model thông tin tài khoản</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-info/{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        public async Task<ActionResult<ApiResultModel>> UpdateUserInfo([FromRoute] string id, [FromBody] UserInfoModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _userService.UpdateUserInfoAsync(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin tài khoản theo id
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-by-info/{userId}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<ApiResultModel<UserCreateModel>>> GetUserInfo([FromRoute] string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _userService.GetUserInfoAsnyc(userId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đăng xuất hệ thống
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("logout")]
        [ActionName(HRMConstants.NoLogEvent)]
        public async Task<ActionResult<ApiResultModel>> LogOutAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _authService.LogOutAsync(CurrentUser.UserId, Request);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}