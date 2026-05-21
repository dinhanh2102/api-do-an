using Foundatio.Caching;
using Microsoft.Extensions.Logging;
using HRM.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Utils
{
    public static class CacheUtil
    {
       
        

        /// <summary>
        /// Xóa cache theo tuyến
        /// </summary>
        /// <param name="cacheClient"></param>
        public async static void ClearTuyen(ICacheClient cacheClient)
        {
            try
            {
                //Clear cache 
                var key = "";

                await cacheClient.RemoveAsync(key);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
