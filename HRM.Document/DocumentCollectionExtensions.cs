using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HRM.Document.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Common.Utils
{
    public static class DocumentCollectionExtensions
    {
        public static IServiceCollection AddDocument(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExcelService, ExcelService>();

            return services;
        }
    }
}
