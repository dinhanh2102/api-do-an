using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HRM.Common.Files
{
    public static class UploadFileCollectionExtensions
    {
        public static IServiceCollection AddHRMUpload(this IServiceCollection services, IConfiguration configuration)
        {
            var uploadSetting = configuration.GetSection("UploadSetting");
            services.Configure<UploadSettingModel>(uploadSetting);

            services.AddSingleton<IUploadFileService, UploadFileService>();

            return services;
        }

        public static IApplicationBuilder UseHRMUploadStaticFiles(this IApplicationBuilder app)
        {
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //     Path.Combine(Directory.GetCurrentDirectory(), "FileUpload/aaa")),
            //    RequestPath = "/FileUpload/aaa"
            //});
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "FileUpload");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "FileUpload")),
                RequestPath = "/FileUpload"
            });

            string exportFolder = Path.Combine(Directory.GetCurrentDirectory(), "Export");
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "Export")),
                RequestPath = "/Export"
            });

            string templateFolder = Path.Combine(Directory.GetCurrentDirectory(), "Template");
            if (!Directory.Exists(templateFolder))
            {
                Directory.CreateDirectory(templateFolder);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "Template")),
                RequestPath = "/Template"
            });

            return app;
        }
    }
}
