using Foundatio.Caching;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HRM.Common.RedisCache;
using HRM.Common.Resource;
using HRM.Common.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Users
{
    public class HRMUserService : IHRMUserService
    {
        private readonly ICacheClient _cacheClient;
        private readonly RedisCacheSettingModel _redisCacheSettings;
        public HRMUserService(ICacheClient cacheClient, IOptions<RedisCacheSettingModel> options)
        {
            _cacheClient = cacheClient;
            _redisCacheSettings = options.Value;
        }

        /// <summary>
        /// Đăng suất
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task Logout(string userId)
        {
            // Tạo key cache
            //string key = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userId}";
            //key menu
            string keymenu = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{userId}";

            // Kiểm tra cache tồn tại
            var keymenuExist = await _cacheClient.ExistsAsync(keymenu);
            if (keymenuExist)
            {
                await _cacheClient.RemoveAsync(keymenu);
            }
            //var keyExist = await _cacheClient.ExistsAsync(key);
            //if (keyExist)
            //{
            //    await _cacheClient.RemoveAsync(key);
            //}

        }

        /// <summary>
        /// Đăng nhập JWT
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<HRMUserTokenModel> HRMJwtLogin(HRMUserLoginModel loginModel, bool refresh = false)
        {
            HRMUserTokenModel userTokenModel = new HRMUserTokenModel();
            if (!refresh)
            {
                var passwordHash = PasswordUtils.ComputeHash(loginModel.Password + loginModel.SecurityStamp);
                if (!loginModel.PasswordHash.Equals(passwordHash))
                {
                    throw HRMException.CreateInstance(MessageResourceKey.MSG0001);
                }
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(loginModel.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginModel.UserId.ToString()),
                    new Claim("Permissions",string.Join(",", loginModel.Permission))
                }),
                Expires = DateTime.UtcNow.AddDays(loginModel.ExpireDateAfter),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            userTokenModel.Name = loginModel.UserName;
            userTokenModel.FullName = loginModel.FullName;
            userTokenModel.UserId = loginModel.UserId;
            userTokenModel.Avatar = loginModel.Avatar;
            userTokenModel.Token = tokenString;
            userTokenModel.ExpireDateAfter = loginModel.ExpireDateAfter;
            userTokenModel.Permissions = loginModel.Permission;
            userTokenModel.RefreshToken = await GenerateRefreshToken();
            userTokenModel.UserGroupId = loginModel.UserGroup;

            // Key lưu cache login
            //string keyLogin = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userTokenModel.UserId}";

            //// Ghi thông tin vào cache
            //await _cacheClient.RemoveAsync(keyLogin);
            //await _cacheClient.AddAsync<HRMUserTokenModel>(keyLogin, userTokenModel, new TimeSpan(loginModel.ExpireDateAfter, 0, 0, 0));

            return userTokenModel;
        }

        public async Task<string> GenerateRefreshToken()
        {
            // generate token that is valid for 7 days
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string ValidateJwtToken(string token, string secret)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
