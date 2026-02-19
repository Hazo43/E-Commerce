using Services;
using Services.Abstractions.Contracts;
using Services.ImplementationService;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(cfg => { }, typeof(AssembluReference).Assembly);
            // ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();
            return services;
        }
    }
}
