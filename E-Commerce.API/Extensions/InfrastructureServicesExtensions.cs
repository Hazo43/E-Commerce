using Domain.Contracs;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.DataSeed;
using Presistence.Data.Dbcontexts;
using Presistence.UnitOfWork;
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

            return services;
        }
    }
}
