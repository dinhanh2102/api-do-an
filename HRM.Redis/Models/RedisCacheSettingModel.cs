using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Common.RedisCache
{
    public class RedisCacheSettingModel : IRedisCacheSettings
    {
        public string PrefixSystemKey { get; set; }
        public string PrefixLoginKey { get; set; }
        public string PrefixOTPKey { get; set; }
        public string PrefixNewKey { get; set; }
        public string PrefixMenuKey { get; set; }
        public string ConnectionString { get; set; }
    }

    public class IRedisCacheSettings
    {
    }
}
