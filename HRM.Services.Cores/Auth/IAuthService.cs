using Microsoft.AspNetCore.Http;
using HRM.Common.Users;
using System.Threading.Tasks;

namespace HRM.Services.Cores.Auth
{
    public interface IAuthService
    {
        Task<HRMUserTokenModel> LoginAsync(HRMLogInModel loginModel, HttpRequest request);

        Task<HRMUserTokenModel> RefreshToken(string userId, string refreshToken);
        //string GetById(string id);

        Task<bool> LogOutAsync(string userId, HttpRequest request);

        /// <summary>
        /// Check token userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsTokenAlive(string userId, string token);

        /// <summary>
        /// Xóa rediscache theo userId
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveRedis(string userId);

    }
}
