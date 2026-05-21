using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HRM.Api.Controllers.Cores
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Thông tin người đăng nhập
        /// </summary>
        protected UserLoginInfo CurrentUser
        {
            get
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                return new UserLoginInfo
                {
                    UserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value,
                    UserName = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value
                };
            }
        }

        /// <summary>
        /// Đã xác thực người dùng chưa
        /// </summary>
        protected bool IsUserAuthenticated
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }
    }

    public class UserLoginInfo
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; }
    }
}
