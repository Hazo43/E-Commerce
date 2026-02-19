
using Domain.Contracs;
using E_Commerce.API.Factories;
using E_Commerce.API.MiddleWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.DataSeed;
using Presistence.Data.Dbcontexts;
using Presistence.UnitOfWork;
using Services;
using Services.Abstractions.Contracts;
using Services.ImplementationService;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region  Add services to the container

            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // validation Error
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // DataSeeding
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            // UnitOfEork
            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            // AutoMapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(AssembluReference).Assembly);
            // ServiceManager
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            #endregion


            var app = builder.Build();

            //Pending Migration ÂÌŒ‘ Â‰« »—œÊ ⁄‘«‰ Ì‘Ê› ·Ê ›ÌÂ «Ì  run ﬂ· „« «·«»·ﬂÌ‘‰ Ì⁄„·
            #region DataSeed
            // DataSeed() «··Ì ÃÊ«Â« «·· ÂÌÂ Method Â—ÊÕ «ﬁ—«¡ «·œ« « „‰ «· DataSeeding Ê„‰ «· DataSeeding Ê„‰Â« ÂÊ’· · GetRequiredService<IDataSeeding>() ⁄‘«‰ «Ê’· · Create Scope  »⁄„·
            using var scope = app.Services.CreateScope();
            var ObjectOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectOfDataSeeding.DataSeedAsync();


            #endregion


            // Configure the HTTP request pipeline.

            #region Configure the HTTP request pipeline.

            // Midleware ==> Handle Exception
            app.UseMiddleware<GlobalExceptionHandlingMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //
            app.UseStaticFiles();
            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
