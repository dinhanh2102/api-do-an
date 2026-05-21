using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Common.RedisCache
{
    public static class RedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("RedisCacheSettings");
            services.Configure<RedisCacheSettingModel>(rabbitConfig);

            services.AddSingleton<IRedisCacheConnectionFactory, RedisCacheConnectionFactory>();
            services.AddSingleton<RedisCacheService>();

            return services;
        }
    }
}
