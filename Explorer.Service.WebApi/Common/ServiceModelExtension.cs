using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Service.WebApi.Common
{
    public static class ServiceModelExtension
    {
        public static IServiceCollection AddServiceModel(this IServiceCollection service)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                throw new DllNotFoundException("the entry dll not be found");
            foreach (Type type in assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IServiceModel))))
            {
                service.AddScoped(type);
            }

            return service;
        }
    }
}