using Foundatio.Caching;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HRM.Common.Models;
using HRM.Common.RedisCache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Cache
{
    public class CacheService: ICacheService
    {
        private readonly ICacheConnection _cacheConnection;
        private ICacheClient cache;
        private readonly AppSettingModel _appSettingModel;
        private readonly RedisCacheSettingModel _redisSettingModel;

        public CacheService(IOptions<AppSettingModel> options, IOptions<RedisCacheSettingModel> redisOptions, ICacheConnection cacheConnection)
        {
            _cacheConnection = cacheConnection;
            _appSettingModel = options.Value;
            _redisSettingModel = redisOptions.Value;
                 
            if (_appSettingModel.Type == 1)
            {
                cache = new RedisCacheClient(t => t.ConnectionMultiplexer(_cacheConnection.Connection()));
            }
            else
            {
                cache = new InMemoryCacheClient();
            }
        }

        public async Task<bool> SetAsync<T>(string key, T data)
        {
            string value = JsonConvert.SerializeObject(data);
            return await cache.SetAsync(key, value);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await cache.GetAsync<T>(key);
            if (!value.HasValue)
            {
                return default(T);
            }
            return value.Value;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await cache.RemoveAsync(key);
        }

        public async Task<bool> ReplaceAsync<T>(string key, T data)
        {
            string value = JsonConvert.SerializeObject(data);
            return await cache.ReplaceAsync(key, value);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await cache.ExistsAsync(key);
        }

        public string SetKey(string preKey, string key)
        {
            return $"{ _redisSettingModel.PrefixSystemKey }{preKey}{key}";
        }
    }
}
