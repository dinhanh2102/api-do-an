using HRM.Models.Entities;
using Foundatio.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HRM.Common;
using HRM.Common.RedisCache;
using HRM.Common.Resource;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models.Cores.Permission;

namespace HRM.Services.Cores.ConfigInterface
{
    public class PermissionService : IPermissionService
    {
        private readonly CoreProjectContext _sqlContext;
        private readonly ICacheClient _cacheClient;
        private readonly RedisCacheSettingModel _redisCacheSettings;

        public PermissionService(CoreProjectContext sqlContext, ICacheClient cacheClient, IOptions<RedisCacheSettingModel> options)
        {
            _sqlContext = sqlContext;
            _cacheClient = cacheClient;
            _redisCacheSettings = options.Value;
        }

        /// <summary>
        /// Danh sách quyền người dùng
        /// </summary>
        /// <param name="searchModel">Điều kiện lọc</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<UserPermissionModel>> GetUserInfoAsync(PermissionSearchModel searchModel)
        {
            SearchBaseResultModel<UserPermissionModel> searchResult = new SearchBaseResultModel<UserPermissionModel>();

            var dataQuery = (from a in _sqlContext.UserPermission.AsNoTracking()
                             join b in _sqlContext.User.AsNoTracking() on a.UserId equals b.Id
                             where searchModel.Code.Equals(a.FunctionId)
                             select new UserPermissionModel
                             {
                                 UserId = b.Id,
                                 UserName = b.UserName,
                                 FullName = b.FullName
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.UserName))
            {
                dataQuery = dataQuery.Where(e => searchModel.UserName.Contains(e.UserName));
            }
            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.OrderBy(e => e.UserName).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Xóa danh sách quyền người dùng
        /// </summary>
        /// <param name="userid">Id tài khoản</param>
        /// <returns></returns>
        public async Task DeletePermissionAsync(string userid)
        {
            var userper = _sqlContext.UserPermission.FirstOrDefault(e => userid.Equals(e.UserId));
            if (userper == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0032);
            }
            _sqlContext.UserPermission.Remove(userper);
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                    //remove cache của user và menu
                    RemoveCache(userid);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa cache người dùng
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        private async Task RemoveCache(string userId)
        {
            // Tạo key cache
            string key = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userId}";
            //key menu
            string keymenu = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{userId}";

            // Kiểm tra cache tồn tại
            var keymenuExist = await _cacheClient.ExistsAsync(keymenu);
            if (keymenuExist)
            {
                await _cacheClient.RemoveAsync(keymenu);
            }
            var keyExist = await _cacheClient.ExistsAsync(key);
            if (keyExist)
            {
                await _cacheClient.RemoveAsync(key);
            }
        }
    }
}
