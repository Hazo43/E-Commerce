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
            // Service Manager With Factory Delegate
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();
            // Product Service
            services.AddScoped<IProductService ,  ProductService>();
            services.AddScoped<Func<IProductService>>(provider =>
            () => provider.GetRequiredService<IProductService>()
            );
            // Basket Service
            services.AddScoped<IBasketService ,  BasketService>();
            services.AddScoped<Func<IBasketService>>(provider =>
            () => provider.GetRequiredService<IBasketService>()
            );
            // IAuthentication Service
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<Func<IAuthenticationService>>(provider =>
            () => provider.GetRequiredService<IAuthenticationService>()
            );
            // IOrder Service
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<Func<IOrderService>>(provider =>
            () => provider.GetRequiredService<IOrderService>()
            );
            // IPayment Service
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<Func<IPaymentService>>(provider =>
            () => provider.GetRequiredService<IPaymentService>()
            );
            // JwtOptions
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}
