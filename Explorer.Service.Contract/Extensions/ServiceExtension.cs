using Explorer.Service.Contract.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Service.Contract.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddContractClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient();
            services.Configure<NgkConfig>(configuration.GetSection("NgkConfig"));

            services.AddSingleton<ContractQueryClient>();

            return services;
        }
    }
}