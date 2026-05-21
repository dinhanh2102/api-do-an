using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Api.Attributes;
using HRM.Models.Cores.GroupUser;
using HRM.Services.Cores.GroupUsers;
using HRM.Common.Models;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Cores
{
    [Route("api/group-users")]
    [ApiController]
    [ValidateModel]
    [Logging]
    [HRMAuthorize]
    [ApiHandleExceptionSystem]
    public class GroupUserController : BaseApiController
    {
        private readonly IGroupUserService groupUser;
        public GroupUserController(IGroupUserService groupUserService)
        {
            groupUser = groupUserService;
        }

        /// <summary>
        /// Tìm kiếm nhóm người dùng
        /// </summary>
        /// <param name="modelSearch">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ActionName(TextResourceKey.Action_Search)]
        [AllowPermission(Permissions = "F0114")]
        public async Task<ActionResult<SearchBaseResultModel<GroupUserResultModel>>> SearchGroupUser([FromBody] GroupUserSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await groupUser.SearchGroupUser(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin nhóm người dùng theo Id
        /// </summary>
        /// <param name="id">Id nhóm người dùng</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-group-user")]
        [ActionName(TextResourceKey.Action_Get)]
        [AllowPermission(Permissions = "F0114;F0112")]
        public async Task<ActionResult<GroupFunctionInfoModel>> GetGroupUserById([FromQuery] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await groupUser.GetGroupUserById(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới nhóm người dùng
        /// </summary>
        /// <param name="model">Model thông tin thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ActionName(TextResourceKey.Action_Create)]
        [AllowPermission(Permissions = "F0111")]
        public async Task<ActionResult<ApiResultModel>> CreateGroupUser([FromBody] GroupFunctionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await groupUser.CreateGroupUser(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập nhóm người dùng
        /// </summary>
        /// <param name="id">Id nhóm người dùng</param>
        /// <param name="model">Model thông tin cập nhật</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0112")]
        public async Task<ActionResult<ApiResultModel>> UpdateGroupUser([FromRoute] string id, [FromBody] GroupFunctionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await groupUser.UpdateGroupUser(id, model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa nhóm người dùng
        /// </summary>
        /// <param name="id">Id nhóm người dùng</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0113")]
        public async Task<ActionResult<ApiResultModel>> DeleteGroupUser([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await groupUser.DeleteGroupUserById(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}