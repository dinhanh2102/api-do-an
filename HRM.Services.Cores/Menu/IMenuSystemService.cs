using HRM.Models.Cores.Function;
using HRM.Models.Cores.Menu;
using HRM.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Services.Cores.Menu
{
    public interface IMenuSystemService
    {   /// <summary>
        /// Tìm kiếm menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<MenuViewModel>> SearchMenuAsync(MenuSearchModel searchModel);

        /// <summary>
        /// Danh sách menu cho left bar
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        Task<List<MenuViewModel>> GetMenuAsync(string userId);

        /// <summary>
        /// Chi tiết menu theo id
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <returns></returns>
        Task<MenuViewModel> GetMenuByIdAsync(string id);

        /// <summary>
        /// Thêm mới menu
        /// </summary>
        /// <param name="model">Model thông tin</param>
        /// <returns></returns>
        Task CreateMenuAsync(CreateMenuModel model);

        /// <summary>
        /// Chỉnh sửa menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <param name="model">Model thông tin</param>
        /// <returns></returns>
        Task UpdateMenuAsync(string id, CreateMenuModel model);

        /// <summary>
        /// Ẩn/Hiện menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <param name="userId">Id người thao tác</param>
        /// <returns></returns>
        Task DisbaleMenuAsync(string id, string userId);

        /// <summary>
        /// Xóa menu
        /// </summary>
        /// <param name="id">Id menu</param>
        /// <param name="userId">Id người thao tác</param>
        /// <returns></returns>
        Task DeleteMenuByIdAsync(string id, string userId);

        /// <summary>
        /// Cập nhật thư tự menu
        /// </summary>
        /// <param name="listMenu">Danh sách menu đã được cập nhật vị trí</param>
        /// <returns></returns>
        Task UpdateIndexMenu(List<UpdateIndexMenuModel> listMenu);

        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        /// <returns></returns>
        Task<List<FunctionAutoModel>> GetListFuntionAuto();
    }
}
