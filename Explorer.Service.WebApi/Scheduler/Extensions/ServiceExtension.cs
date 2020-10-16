using Explorer.Service.WebApi.Scheduler.Config;
using Thor.Framework.Common.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Service.WebApi.Scheduler.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<QuartzOptions>(configuration.GetSection("QuartzConfig"))
                .AddSingleton<QuartzStartup>();
        }

        public static IApplicationBuilder UseQuartz(this IApplicationBuilder app)
        {
            var startup = AspectCoreContainer.Resolve<QuartzStartup>();
            startup?.Init();
            return app;
        }
    }
}