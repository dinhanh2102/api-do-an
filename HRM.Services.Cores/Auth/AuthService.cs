using HRM.Models.Entities;
using HRM.Services.Cores.Log;
using Foundatio.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HRM.Common;
using HRM.Common.Models;
using HRM.Common.RedisCache;
using HRM.Common.Resource;
using HRM.Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models.Cores.UserHistory;

namespace HRM.Services.Cores.Auth
{
    public class AuthService : IAuthService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly AppSettingModel _appSettingModel;
        private readonly IHRMUserService _HRMUserService;
        private readonly ICacheClient _cacheClient;
        private readonly RedisCacheSettingModel _redisCacheSettingModel;
        private readonly ILogEveHRMervice _logEveHRMervice;

        public AuthService(CoreProjectContext sqlContext, IHRMUserService HRMUserService,
            IOptions<AppSettingModel> options, IOptions<RedisCacheSettingModel> redisOptions, ICacheClient cacheClient, ILogEveHRMervice logEveHRMervice)
        {
            _sqlContext = sqlContext;
            _appSettingModel = options.Value;
            _HRMUserService = HRMUserService;
            _cacheClient = cacheClient;
            _redisCacheSettingModel = redisOptions.Value;
            _logEveHRMervice = logEveHRMervice;
        }

        public async Task<HRMUserTokenModel> LoginAsync(HRMLogInModel loginModel, HttpRequest request)
        {
            var user = (from u in _sqlContext.User.AsNoTracking()
                        where u.UserName.Equals(loginModel.Username)
                        select new HRMUserLoginModel
                        {
                            UserId = u.Id,
                            UserName = u.UserName,
                            FullName = u.FullName,
                            PasswordHash = u.PasswordHash,
                            SecurityStamp = u.SecurityStamp,
                            LockoutEnabled = u.LockoutEnabled,
                            Avatar = u.Avatar,
                            UserGroup = u.UserGroupId,
                            //Type = u.Type,
                            //TinhId = u.TinhId,
                            //HuyenId = u.HuyenId,
                            //XaId = u.XaId
                        }).FirstOrDefault();

            if (user == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0001);
            }

            if (user.LockoutEnabled)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0002);
            }

            user.Permission = GetListPermission(user.UserId);
            user.Password = loginModel.Password;
            user.ExpireDateAfter = _appSettingModel.ExpireDateAfter;
            user.Secret = _appSettingModel.Secret;
            //var refreshtokenold = _sqlContext.RefreshToken.FirstOrDefault(e => e.UserId.Equals(user.UserId));
            //if (refreshtokenold != null) _sqlContext.RefreshToken.Remove(refreshtokenold);
            var userToken = await _HRMUserService.HRMJwtLogin(user);
            //SaveRefreshToken(user.UserId, userToken.RefreshToken);

            loginModel.Password = "";//không lưu password vào lịch sử
                                     //UserHistoryModel userHistoryModel = new UserHistoryModel()
                                     //{
                                     //    Name = "Đăng nhập hệ thống",
                                     //    Content = JsonConvert.SerializeObject(loginModel),
                                     //    Type = HRMConstants.UserHistory_Type_Login
                                     //};
                                     //_logEveHRMervice.LogEventAsync(request, userHistoryModel, user.UserId);

            _sqlContext.SaveChanges();
            return userToken;
        }
        public async Task<HRMUserTokenModel> RefreshToken(string userId, string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0003);
            }
            var refreshtoken = _sqlContext.RefreshToken.FirstOrDefault(e => e.UserId.Equals(userId));

            if (refreshtoken == null || refreshtoken.Token != refreshToken || refreshtoken.ExpireAt <= DateTime.Now)
                throw HRMException.CreateInstance(MessageResourceKey.MSG0003);
            var user = new HRMUserLoginModel()
            {
                Permission = GetListPermission(userId),
                ExpireDateAfter = _appSettingModel.ExpireDateAfter,
                Secret = _appSettingModel.Secret,
                UserId = userId,
            };
            var userToken = await _HRMUserService.HRMJwtLogin(user, true);
            _sqlContext.RefreshToken.Remove(refreshtoken);
            SaveRefreshToken(user.UserId, userToken.RefreshToken);
            _sqlContext.SaveChanges();

            return userToken;
        }
        private void SaveRefreshToken(string userid, string tokenrefresh)
        {
            RefreshToken refreshtoken = new RefreshToken()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userid,
                Token = tokenrefresh,
                IssueAt = DateTime.Now.AddDays(_appSettingModel.IssuedAt),
                ExpireAt = DateTime.Now.AddDays(_appSettingModel.ExpiresAt),
            };
            _sqlContext.RefreshToken.Add(refreshtoken);

        }

        /// <summary>
        /// Lấy danh sách quyền cho người dùng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> GetListPermission(string userId)
        {
            List<string> listPermission = new List<string>();

            //Nếu là tài khoản admin hoặc root
            if (HRMConstants.IdUserAdminFix.Equals(userId) || HRMConstants.IdUserRootFix.Equals(userId))
            {
                listPermission = _sqlContext.Function.AsNoTracking().Select(x => x.Code).ToList();
            }
            else
            {
                listPermission = (from a in _sqlContext.UserPermission.AsNoTracking()
                                  join b in _sqlContext.Function.AsNoTracking() on a.FunctionId equals b.Id
                                  where a.UserId.Equals(userId)
                                  select b.Code).ToList();
            }
            return listPermission;
        }

        /// <summary>
        /// Đăng xuất hệ thống
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> LogOutAsync(string userId, HttpRequest request)
        {
            await _HRMUserService.Logout(userId);

            UserHistoryModel userHistoryModel = new UserHistoryModel()
            {
                Name = "Đăng xuất hệ thống",
                Type = HRMConstants.UserHistory_Type_Login
            };
            var refreshtoken = _sqlContext.RefreshToken.FirstOrDefault(e => e.UserId.Equals(userId));
            if (refreshtoken != null) _sqlContext.RefreshToken.Remove(refreshtoken);
            _logEveHRMervice.LogEventAsync(request, userHistoryModel, userId);
            _sqlContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Check token userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsTokenAlive(string userId, string token)
        {
            // Key lưu cache login
            string keyLogin = $"{_redisCacheSettingModel.PrefixSystemKey}{_redisCacheSettingModel.PrefixLoginKey}{userId}";
            bool isToken = false;
            CacheValue<HRMUserTokenModel> cachedValue = await _cacheClient.GetAsync<HRMUserTokenModel>(keyLogin);
            if (cachedValue.HasValue)
            {
                try
                {
                    isToken = token.Equals(cachedValue.Value.Token);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return isToken;
        }

        /// <summary>
        /// Xóa rediscache theo userId
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveRedis(string userId)
        {
            // Key lưu cache login
            string keyLogin = $"{_redisCacheSettingModel.PrefixSystemKey}{_redisCacheSettingModel.PrefixLoginKey}{userId}";
            if (_cacheClient.ExistsAsync(keyLogin).Result)
            {
                var IsCheck = _cacheClient.RemoveAsync(keyLogin).Result;
            }
        }
    }
}
