using Domain.Contracs;
using Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Dbcontexts;
using System.Text.Json;



namespace Presistence.Data.DataSeed
{
    public class DataSeeding : IDataSeeding
    {

        private readonly StoreDbContext _dbContext;

        public DataSeeding(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigration = await _dbContext.Database.GetPendingMigrationsAsync();

                // Database في ال Apply هروح اعملهم  Pending Migration لو فيه اي 
                if (PendingMigration.Any())
                {
                    // لو لسه مش موجوده Database لل Create هتعمل
                   await _dbContext.Database.MigrateAsync();
                }

                //ل روح اقراء او ضيف الداتا ProductBrands لو مفهوش اي
                if (!_dbContext.ProductBrands.Any())
                {
                    // بيقرا الداتا
                    var ProductBrandsData = File.OpenRead("..\\Infrastructure\\Presistence\\Data\\DataSeed\\JsonFile\\brands.json");
                    // [List<ProductBrand>] C# Object الي JSON من (ProductBrandData) هحول ال
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandsData);
                    // بعمل اتشك لو فيه اي داتا حتي لو واحده ع الاقل رو ضيفها
                    if (ProductBrands is not null && ProductBrands.Any())
                       await _dbContext.ProductBrands.AddRangeAsync(ProductBrands);
                }

                //ل روح اقراء او ضيف الداتا ProductTypes لو مفهوش اي
                if (!_dbContext.ProductTypes.Any())
                {
                    var ProductTypesData = File.OpenRead("..\\Infrastructure\\Presistence\\Data\\DataSeed\\JsonFile\\types.json");
                    // [List<ProductType>] C# Object الي JSON من (ProductTypeData) هحول ال
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypesData);
                    if (ProductTypes is not null && ProductTypes.Any())
                       await _dbContext.ProductTypes.AddRangeAsync(ProductTypes);
                }

                //ل روح اقراء او ضيف الداتا Products لو مفهوش اي
                if (!_dbContext.Products.Any())
                {
                    // بيقرا الداتا
                    var ProductData = File.OpenRead("..\\Infrastructure\\Presistence\\Data\\DataSeed\\JsonFile\\products.json");
                    // [List<Products>] C# Object الي JSON من (Products) هحول ال
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);
                    // بعمل اتشك لو فيه اي داتا حتي لو واحده ع الاقل رو ضيفها
                    if (Products is not null && Products.Any())
                       await _dbContext.Products.AddRangeAsync(Products);

                }

               await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($" Data Seeding Failed : {ex}");
            }
            
        }


    }
}
