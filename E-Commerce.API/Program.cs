
using Domain.Contracs;
using E_Commerce.API.Extensions;
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
            #region DI Container

            var builder = WebApplication.CreateBuilder(args);

            // WepApi Services ==> WepApiServices Ïí method áæÍÏåÇ áæ ÚæÒÊ ÇÖíÝ Çí ÍÊÌå ÇÖíÝ Ýí Çámethod ÚãáÊåÇ Ýí WepApi Services Çí ÍÇÌå ÊÈÚ Çá
            builder.Services.WepApiServices();

            // Infrastructure Services ==> AddInfrastructureServices Ïí method áæÍÏåÇ áæ ÚæÒÊ ÇÖíÝ Çí ÍÊÌå ÇÖíÝ Ýí Çámethod ÚãáÊåÇ Ýí Infrastructure Services Çí ÍÇÌå ÊÈÚ Çá
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Core Services ==> AddCoreService Ïí method áæÍÏåÇ áæ ÚæÒÊ ÇÖíÝ Çí ÍÊÌå ÇÖíÝ Ýí Çámethod ÚãáÊåÇ Ýí Core Services Çí ÍÇÌå ÊÈÚ Çá
            builder.Services.AddCoreService();


            #endregion

            var app = builder.Build();
            // DataSeeding åíå Çááí ÔÇíáå Çá SeedDatabaseAsync Ïí  method Çá
            await app.SeedDatabaseAsync();



            // Configure the HTTP request pipeline.

            #region Pipelines - MiddleWares.

            // Midleware ==> Handle Exception
            app.UseExceptionHandleMiddleWares();

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
