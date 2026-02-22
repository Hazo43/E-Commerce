using Domain.Contracs;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presistence.Data.DataSeed;
using Presistence.Data.Dbcontexts;
using Presistence.Identity.DbContext;
using Presistence.Repositories;
using Presistence.UnitOfWork;
using Shared.Common;
using StackExchange.Redis;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce.API.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            // StoreDbContext
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            // IdentityDbContext
            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
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
            // User Identity Role 
            services.AddIdentityCore<User>( options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityStoreDbContext>();
            // Validate JWt 
            services.ValidateJwt(configuration);
            return services;
        }
     
        public static IServiceCollection ValidateJwt(this IServiceCollection services , IConfiguration configuration)
        {
            // عشان استخدمهم في التحقيق jwtOptions و بحطهي في كلاس (SecretKey , Audience , Issuer) و هاخد منها القيم زي appsettings.json هنا انا وصلت ل
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            // عليها Validate اللي من خلالها هوصل لكل حاجه عاوز ابدا ان انا اعمل function دي ال
            services.AddAuthentication(options =>
            {
                // ولا لا اساسا Token  اساسا ولا لا يعني انت معاكLogin ان الشخص دا عامل  Validate معناها ان انا عاوز اعمل DefaultAuthenticateScheme دي 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              
                // علي حسب حاله الايرور بتاعو Error اساسا ف انا محتاج اهندل الكلام دا و هنرجعلو  Token في حاله ان الشخص عندو مشكله او حصل مشكله معينه زي مثلا هو جاي من غير DefaultChallengeScheme و دي
                // C# و الايرور بيتهندل من اللغه نفسها ال
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer( options =>
            {
                // عشان اتاكد ان مش اي حد يخش عليها Token اللي عندي في ال Parameters  علي كا ال Validation كل دا بقولو روح اعمل
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // دول بديلو الاذن ان هو يفحصهم
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // و الحجات دي عشان يعرف هل هما زي بعض ولا لا jwtOptions.Issuer هنا بعرفلو ال
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };
            });

            services.AddAuthorization();

            return services;

        }

    }
}
