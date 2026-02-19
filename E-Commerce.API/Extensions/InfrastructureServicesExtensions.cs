using Domain.Contracs;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.DataSeed;
using Presistence.Data.Dbcontexts;
using Presistence.Repositories;
using Presistence.UnitOfWork;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce.API.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            // DataSeeding
            services.AddScoped<IDataSeeding, DataSeeding>();
            // UnitOfEork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Basket 
            services.AddSingleton<IConnectionMultiplexer>(SP =>
            {
               return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            return services;
        }
    }
}
