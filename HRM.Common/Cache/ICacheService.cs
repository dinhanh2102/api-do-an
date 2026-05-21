using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Cache
{
    public interface ICacheService
    {
        Task<bool> SetAsync<T>(string key, T data);

        Task<T> GetAsync<T>(string key);

        Task<bool> RemoveAsync(string key);

        Task<bool> ReplaceAsync<T>(string key, T data);

        Task<bool> ExistsAsync(string key);

        string SetKey(string preKey, string key);
    }
}
