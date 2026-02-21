using Domain.Contracs;
using E_Commerce.API.MiddleWares;
using Microsoft.AspNetCore.Builder;

namespace E_Commerce.API.Extensions
{
    public static class WepApplicationExtensions
    {
        public static async Task<WebApplication> SeedDatabaseAsync( this WebApplication app)
        {
            //Pending Migration هيخش هنا بردو عشان يشوف لو فيه اي  run كل ما الابلكيشن يعمل
            // DataSeed() اللي جواها الل هيه Method هروح اقراء الداتا من ال DataSeeding ومن ال DataSeeding ومنها هوصل ل GetRequiredService<IDataSeeding>() عشان اوصل ل Create Scope  بعمل
            using var scope = app.Services.CreateScope();
            var ObjectOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectOfDataSeeding.DataSeedAsync();
            // IdentitySeed
            await ObjectOfDataSeeding.SeedIdentityDataAsync();
            return app;
        }

        public static WebApplication UseExceptionHandleMiddleWares( this WebApplication app )
        {
            // Midleware ==> Handle Exception
            app.UseMiddleware<GlobalExceptionHandlingMiddleWare>();
            return app;
        }
    }
}
