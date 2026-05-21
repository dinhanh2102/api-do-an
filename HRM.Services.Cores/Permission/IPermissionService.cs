using HRM.Models.Cores.Permission;
using HRM.Common.Models;
using System.Threading.Tasks;

namespace HRM.Services.Cores.ConfigInterface
{
    public interface IPermissionService
    {
        /// <summary>
        /// Dánh sách quyền người dùng
        /// </summary>
        /// <param name="modelSearch">Diều kiện tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<UserPermissionModel>> GetUserInfoAsync(PermissionSearchModel modelSearch);

        /// <summary>
        /// Xóa quyền người dùng
        /// </summary>
        /// <param name="userid">Id tài khoản</param>
        /// <returns></returns>
        Task DeletePermissionAsync(string userid);
    }
}
