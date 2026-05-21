using HRM.Common;
using HRM.Common.Models;
using HRM.Models.Cores.Function;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HRM.Models.Cores.User;
using HRM.Models.Cores.GroupFunction;

namespace HRM.Services.Cores.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Tìm kiếm tài khoản
        /// </summary>
        /// <param name="searchModel">Model điều kiện tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<UserSearchResultModel>> SearchUserAsync(UserSearchModel searchModel);

        /// <summary>
        /// Thêm mới tài khoản
        /// </summary>
        /// <param name="model">Model thông tin thêm mới</param>
        /// <param name="userId">Id người thêm mới</param>
        /// <returns></returns>
        Task CreateUserAsync(UserCreateModel model, string userId);

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="model">Model thông tin cập nhật</param>
        /// <param name="userId">Id người cập nhật</param>
        /// <returns></returns>
        Task UpdateUserAsync(string id, UserCreateModel model, string userId);

        /// <summary>
        /// Mở/Khóa tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="isunlock">Trạng thái</param>
        /// <returns></returns>
        Task UserLockAsync(string id, bool isunlock);

        /// <summary>
        ///  Lấy thông tin tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <returns></returns>
        Task<UserCreateModel> GetUserByIdAsnyc(string id);

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="id">Id tài khoản</param>
        /// <param name="userId">Id người xóa</param>
        /// <returns></returns>
        Task DeleteUserAsync(string id, string userId);

        /// <summary>
        /// Lấy quyền danh sách quyền
        /// </summary>
        /// <param name="groupUserId">Id nhóm quyền</param>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        Task<List<GroupFunctionModel>> GetPermissionAsync(string groupUserId, string userId = null);

        /// <summary>
        /// Đổi mật khẩu người dùng
        /// </summary>
        /// <param name="model">Thông tin mật khẩu thay đổi</param>
        /// <returns></returns>
        Task ChangePasswordAsync(ChangePasswordModel model);

        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        Task<UserInfoModel> GetUserInfoAsnyc(string userId);

        /// <summary>
        /// Cập nhật người dùng
        /// </summary>
        /// <param name="id">Id người dùng</param>
        /// <param name="model">Model thông tin cập nhật</param>
        /// <param name="userId">Id người cập nhật</param>
        /// <returns></returns>
        Task UpdateUserInfoAsync(string id, UserInfoModel model, string userId);
    }
}
