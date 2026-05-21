using HRM.Services.Cores.ConfigInterface;
using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Common.Models;
using System.Threading.Tasks;
using HRM.Models.Cores.Permission;

namespace HRM.Api.Controllers.Cores
{

    [Route("api/permission")]
    [ApiController]
    [HRMAuthorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class PermissionManagementController : BaseApiController
    {
        private readonly IPermissionService _permissionService;
        public PermissionManagementController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        [Route("user-info")]
        [AllowPermission(Permissions = "F0171")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult<SearchBaseResultModel<UserPermissionModel>>> GetUserInfo([FromBody] PermissionSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _permissionService.GetUserInfoAsync(modelSearch);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("user-delete/{userid}")]
        [AllowPermission(Permissions = "F0172")]
        [ActionName(TextResourceKey.Action_Search)]
        public async Task<ActionResult> DeletePermission(string userid)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _permissionService.DeletePermissionAsync(userid);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
