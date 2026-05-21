using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Users
{
    public interface IHRMUserService
    {
        /// <summary>
        /// Auth login Jwt
        /// </summary>
        /// <param name = "loginModel" ></ param >
        /// < returns ></ returns >
        Task<HRMUserTokenModel> HRMJwtLogin(HRMUserLoginModel loginModel, bool refresh = false);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token, string secret);
        Task Logout(string token);

        Task<string> GenerateRefreshToken();

        string ValidateJwtToken(string token, string secret);
    }
}
