using Services;
using Services.Abstractions.Contracts;
using Services.ImplementationService;
using Shared.Common;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services , IConfiguration configuration)
        {
            // AutoMapper
            services.AddAutoMapper(cfg => { }, typeof(AssembluReference).Assembly);
            // ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();
            // JwtOptions
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}
