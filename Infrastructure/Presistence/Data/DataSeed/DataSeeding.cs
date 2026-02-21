using Domain.Contracs;
using Domain.Entities.IdentityModule;
using Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Dbcontexts;
using System.Text.Json;



namespace Presistence.Data.DataSeed
{
    public class DataSeeding : IDataSeeding
    {

        private readonly StoreDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DataSeeding(StoreDbContext dbContext ,
                           RoleManager<IdentityRole> roleManager ,
                           UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task SeedIdentityDataAsync()
        {
             try
             {
                //1] Seed Roles [ Admin , SuperAdmin]
                if(! _roleManager.Roles.Any()) // seed لو مفهوش اي داتا روح اعمل
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                
                //2] Seed Users [ UserAdmin , UserSuperAdmin ]
                if(! _userManager.Users.Any())// seed لو مفهوش اي داتا روح اعمل
                {
                    var adminUser = new User()
                    {
                        DisplayName = "Ali",
                        UserName = "Ali",
                        Email = "Ali@gmail.com",
                        PhoneNumber = "01234567891"
                    };
                    var superAdminUser = new User()
                    {
                        DisplayName = "Hazo",
                        UserName = "Hazo",
                        Email = "Hazo@gmail.com",
                        PhoneNumber = "01234567881"
                    };
                     // اللي عندنا user ل ال Create بنعمل
                     await _userManager.CreateAsync(adminUser, "P@ssw0rd");
                     await _userManager.CreateAsync(superAdminUser, "P@ssw0rd");

                    //3] Assign Roles ==> Users   معينه Role هخلي كل واحد عندو
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }


            }
            catch (Exception ex)
             {
                throw;
             }
        }
    }
}
