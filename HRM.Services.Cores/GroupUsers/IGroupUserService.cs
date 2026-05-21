using HRM.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models.Cores.GroupUser;

namespace HRM.Services.Cores.GroupUsers
{
    public interface IGroupUserService
    {
        /// <summary>
        /// Tìm kiếm nhóm người dùng
        /// </summary>
        /// <param name="modelSearch">Điều kiện tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<GroupUserResultModel>> SearchGroupUser(GroupUserSearchModel modelSearch);

        /// <summary>
        /// Thêm mới nhóm người dùng
        /// </summary>
        /// <param name="model">Thông tin thêm mới</param>
        /// <param name="userId">Id người thao tác</param>
        /// <returns></returns>
        Task CreateGroupUser(GroupFunctionCreateModel model, string userId);

        /// <summary>
        /// Xóa nhóm người dùng
        /// </summary>
        /// <param name="id">Id nhóm người dùng</param>
        /// <param name="userId">Id người thao tác</param>
        /// <returns></returns>
        Task DeleteGroupUserById(string id, string userId);

        /// <summary>
        /// Lấy dữ liệu nhóm người dùng
        /// </summary>
        /// <param name="id">Id nhóm người dùng</param>
        /// <returns></returns>
        Task<GroupUserModel> GetGroupUserById(string id);

        /// <summary>
        /// Cập nhật nhóm người dùng
        /// </summary>
        /// <param name="id">Id nhóm quyền</param>
        /// <param name="model">Model thông tin</param>
        /// <param name="updateby">Id người thao tác</param>
        /// <returns></returns>
        Task UpdateGroupUser(string id, GroupFunctionCreateModel model, string updateby);

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        /// <param name="lstCheck">Dánh quyền được chọn</param>
        /// <returns></returns>
        List<PermissionModel> GetListGroupPermissions(List<string> lstCheck);

    }
}
