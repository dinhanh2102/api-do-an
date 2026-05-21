using Foundatio.Caching;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using HRM.Common.Attributes;
using HRM.Common.Files;
using HRM.Common.Models;
using HRM.Common.RedisCache;
using HRM.Common.Users;
using HRM.Document.Excel;
using HRM.Document.Word;
using HRM.Api.AppInitialize;
using HRM.Api.Middleware;
using HRM.Models.Entities;
using HRM.Models.Helpers;
using HRM.Services.Cores.Auth;
using HRM.Services.Cores.Combobox;
using HRM.Services.Cores.ConfigInterface;
using HRM.Services.Cores.FileView;
using HRM.Services.Cores.GroupUsers;
using HRM.Services.Cores.Log;
using HRM.Services.Cores.Menu;
using HRM.Services.Cores.Signalr;
using HRM.Services.Cores.UserHistorys;
using HRM.Services.Cores.Users;
using HRM.Services.Cores.ViewFileWeb;
using StackExchange.Redis;
using System.Linq;
using System.Text;
using HRM.Services.Projects.NhanVien;
using HRM.Services.Projects.LoaiCa;
using HRM.Services.Projects.LoaiCong;
using HRM.Services.Projects.DonViPhongBan;
using HRM.Services.Projects.ChucDanh;
using HRM.Services.Projects.KyCong;
using HRM.Services.Projects.ChamCong;
using HRM.Services.Projects.LoaiPhuCap;
using HRM.Services.Projects.PhuCapNhanVien;
using HRM.Services.Projects.BangLuong;

namespace HRM.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowAll", p =>
                {
                    p.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
                    //.WithOrigins("http://localhost:3456")
                    //.AllowCredentials();
                });
            });

            services.AddDbContext<CoreProjectContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.None; // Set HttpOnly to false
            });

            // Add detection services container and device resolver service.
            services.AddDetection();

            // Add framework services.
            services.AddControllersWithViews();
            services.AddSession();

            services.AddSignalR();

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 4294967296;
            });

            var appSettingsSection = Configuration.GetSection("AppSetting");
            services.Configure<AppSettingModel>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettingModel>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            //services.AddCache(Configuration);

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddHRMUpload(Configuration);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRM.API", Version = "v1" });
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(new ApiHandleExceptionSystemAttribute());
            })
                 .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                 .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            var connectionStringsSection = Configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStringModel>(connectionStringsSection);

            var redisSettingSection = Configuration.GetSection("RedisCacheSettings");
            services.Configure<RedisCacheSettingModel>(redisSettingSection);

            var redisConfig = redisSettingSection.Get<RedisCacheSettingModel>();
            var muxer = ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
            services.AddSingleton<ICacheClient>(s => new RedisCacheClient(new RedisCacheClientOptions { ConnectionMultiplexer = muxer }));

            services.AddScoped<IComboboxService, ComboboxService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHRMUserService, HRMUserService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupUserService, GroupUserService>();
            services.AddScoped<ILogEveHRMervice, LogEveHRMervice>();
            services.AddScoped<IUserHistoryService, UserHistoryService>();
            services.AddScoped<IWordService, WordService>();
            services.AddScoped<IViewFileWebService, ViewFileWebService>();
            services.AddScoped<IFileViewService, FileViewService>();
            services.AddScoped<IMenuSystemService, MenuSystemService>();
            services.AddScoped<IConfigInterfaceService, ConfigInterfaceService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<INhanVienService, NhanVienService>();
            services.AddScoped<ILoaiCaService, LoaiCaService>();
            services.AddScoped<ILoaiCongService, LoaiCongService>();
            services.AddScoped<IDonViPhongBanService, DonViPhongBanService>();
            services.AddScoped<IChucDanhService, ChucDanhService>();
            services.AddScoped<IKyCongService, KyCongService>();
            services.AddScoped<IChamCongService, ChamCongService>();
            services.AddScoped<IPhuCapService, PhuCapService>();
            services.AddScoped<IPhuCapNhanVienService, PhuCapNhanVienService>();
            services.AddScoped<IBangLuongService, BangLuongService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("MyAllowAll");

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTgxNDc2QDMxMzkyZTM0MmUzMGR1VkRzdTNobWNrRW54eERGc0dKUzM4SlRlU21XV3NuV2UyYTdkUUFmQTA9;NTgxNDc3QDMxMzkyZTM0MmUzMGJQa3VmcFU1b0lSemIyM3JHYzZpVWR3T3dzZTBJUThwRGxldHgvYWN3WEk9;NTgxNDc4QDMxMzkyZTM0MmUzMEJyeUFGYVIxZ0w1cUZ2OGtQVnI5VHBISXdDZGF6a1B1Q0ZTYVgvMUdXOVE9;NTgxNDc5QDMxMzkyZTM0MmUzMEVaSnFiRUYyZ0EwMW1PWDZmd282MjZ5aWRzRWxKSUE1S1RNYlAyVlBCZlE9;NTgxNDgwQDMxMzkyZTM0MmUzMGJVbzZxUmhOQWNORUlhZ0Evc1E4dTFCTktwbmZZeHRoT1pWZDhzdThVeG89;NTgxNDgxQDMxMzkyZTM0MmUzMGtzWisrb1ZEVG5NSHI5a0ZFZUpHdW4zTHU5SHlRdS9YSXpKWHg0VjVlRkE9;NTgxNDgyQDMxMzkyZTM0MmUzMGtnZXRGTWJLdlk1bkVnVlB5RW5vc005MEpqc1ROQ3F2a2JtdVNkMUNGMHM9;NTgxNDgzQDMxMzkyZTM0MmUzMEdOTGtpRVg4WnoxY25MZ3hzYjhaYS9OVFM5ZUM2MWZNMmVaK2pYNm5FcTQ9;NTgxNDg0QDMxMzkyZTM0MmUzME5qVk8xWmhHN0x3dGFWY202OHBXV28zU25DalVRdXcybFdQV0pHTTVIWFk9;NTgxNDg1QDMxMzkyZTM0MmUzMEZjalpnN2J3c3ZKdm5QTWhKU3RQY0xXSk05N2RLb2E4bmFiN2hFckR2Vm89;NTgxNDg2QDMxMzkyZTM0MmUzMFB0L0J0WHJYc1lxUGpFU1VxWnVZQXpCRGc3LzUvQnhQRTFGd0JQVWdueHM9");
            InitializeDatabase(app);

            //Để phát triển
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRM.API v1"));
            }

            loggerFactory.AddFile("Logs/log-{Date}.txt");
            app.UseHttpsRedirection();
            app.UseAntiXssMiddleware();
            app.UseStaticFiles();
            app.Use((context, next) =>
   {
       context.Request.EnableBuffering();
       return next();
   });

            app.UseAuthentication();

            app.UseDetection();
            app.UseSession();
            app.UseFileServer();
            app.UseRouting();
            app.UseHRMUploadStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoiHRM =>
            {
                //endpoiHRM.MapControllers();
                endpoiHRM.MapHub<SignalrHub>("/signalr");

                endpoiHRM.MapControllerRoute(
                    name: "default",
                  pattern: "{controller=angularhome}/{action=index}/{id?}"
                );
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CoreProjectContext>();
                if (context.Database.GetMigrations().Any())
                {
                    context.Database.Migrate();
                }
                else
                {
                    context.Database.EnsureCreated();
                }

                InitSystemData.Init(context);
            }
        }
    }
}
