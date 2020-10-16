using System;
using AspectCore.Extensions.DependencyInjection;
using Explorer.Service.Contract.Extensions;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.WebApi.Common;
using Thor.Framework.Common.IoC;
using Thor.Framework.Common.Log;
using Thor.Framework.Common.Options;
using Thor.Framework.Data.DbContext.Relational;
using Thor.Framework.Service.WebApi.Middleware;
using Explorer.Service.WebApi.Hubs;
using Explorer.Service.WebApi.Scheduler.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Explorer.Service.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 缓存

            services.AddMemoryCache();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"];
                options.InstanceName = Configuration["Redis:Instance"];
            });

            #endregion

            #region 注入Database

            var settings = Configuration.Get<CustomerSettings>();

            foreach (var dbSetting in settings.DatabaseSettings)
            {
                switch (dbSetting.DatabaseType)
                {
                    case "Postgresql":
                        services.AddDbContext<IDbContextCore, ChainContext>(ServiceLifetime.Scoped);

                        services.Configure<DbContextOption>(option =>
                        {
                            option.IsCodeFirst = dbSetting.IsCodeFirst;
                            option.ModelAssemblyName = dbSetting.ModelAssemblyName;
                            option.ConnectionString = dbSetting.ConnectionString;
                            option.DatabaseType = dbSetting.DatabaseType;
                        });
                        break;
                    default:
                        throw new Exception("未能找到相应的数据库连接！");
                }
            }

            #endregion

            #region 其他注入

            services.AddSingleton(Configuration)
                .AddScopedAssembly("Explorer.Service.DataAccess.Interface",
                    "Explorer.Service.DataAccess.Implement");

            services.AddServiceModel();

            services.AddMvc()
                .AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod()
                        .AllowCredentials());
            });

            services.AddOptions();
            services.AddAspectCoreContainer();
            services.AddContractClient(Configuration);
            services.AddQuartz(Configuration);
            services.AddSignalR();

            #endregion

            return AspectCoreContainer.BuildServiceProvider(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            app.UseCors();
            app.UseGlobalExceptionHandler();
            app.UseQuartz();
            app.UseSignalR(routes => routes.MapHub<BlockHub>("/hub/last-block"));
            app.UseMvcWithDefaultRoute();
        }
    }
}