using Microsoft.AspNetCore.Mvc;
using HRM.Common.Attributes;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Cores.Menu;
using HRM.Services.Cores.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Api.Controllers.Cores
{

    [Route("api/menu")]
    [ApiController]
    [HRMAuthorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class MenuSystemController : BaseApiController
    {
        private readonly IMenuSystemService _menuService;
        public MenuSystemController(IMenuSystemService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// Tìm kiếm menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [AllowPermission(Permissions = "F0005")]
        public async Task<ActionResult<ApiResultModel>> SearchMenu([FromBody] MenuSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            //apiResultModel.Data = await _menuService.GetMenuAsync(GetUserIdByRequest());
            apiResultModel.Data = await _menuService.SearchMenuAsync(searchModel);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách menu cho left bar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-menu-user")]
        public async Task<ActionResult<ApiResultModel>> GetMenu()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _menuService.GetMenuAsync(CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy chi tiết menu theo id
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id/{id}")]
        [AllowPermission(Permissions = "F0005")]
        public async Task<ActionResult<ApiResultModel>> GetMenuById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _menuService.GetMenuByIdAsync(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới menu
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [AllowPermission(Permissions = "F0001")]
        public async Task<ActionResult<ApiResultModel>> CreateMenu(CreateMenuModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.CreateMenuAsync(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Sửa menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <param name="model">Model thông tin chỉnh sửa</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        [AllowPermission(Permissions = "F0002")]
        public async Task<ActionResult<ApiResultModel>> UpdateMenu([FromRoute] string id, [FromBody] CreateMenuModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.UpdateMenuAsync(id, model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Ẩn/Hiện menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <returns></returns>
        [HttpPut]
        [Route("disable/{id}")]
        [AllowPermission(Permissions = "F0004")]
        public async Task<ActionResult<ApiResultModel>> DisableMenu([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.DisbaleMenuAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ActionName(TextResourceKey.Action_Delete)]
        [AllowPermission(Permissions = "F0003")]
        public async Task<ActionResult<ApiResultModel>> DeleteMenu([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            await _menuService.DeleteMenuByIdAsync(id, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật vị trí hiển thị menu
        /// </summary>
        /// <param name="model">Danh sách menu được sắp xếp</param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-index")]
        [AllowPermission(Permissions = "F0002")]
        public async Task<ActionResult<ApiResultModel>> UpdateIndexMenu(List<UpdateIndexMenuModel> model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _menuService.UpdateIndexMenu(model);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list-funtion-auto")]
        public async Task<ActionResult<ApiResultModel>> GetListFuntionAuto()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _menuService.GetListFuntionAuto();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
