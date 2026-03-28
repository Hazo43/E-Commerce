using AdminDashboard.Models.Roles;
using AdminDashboard.Models.Users;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // Index
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserViewModel()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Username = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result,
            }).ToListAsync();

            return View(users);
        }

        // Edit --> Get
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var roles = await _roleManager.Roles.ToListAsync();

            var userModel = new UserRoleViewModel()
            {
                UserId = id,
                Username = user.UserName!,

                // دي ولا لا role يعني عندو ال IsSelected عندو ال user و هنشوف ال RoleName و RoleId  نشوف ال Role هنلف ع كل 
                Roles = roles.Select(r => new UpdateRoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    // دي علم عليها role عندو ال user دي بتقولو لو ال
                    IsSelected = _userManager.IsInRoleAsync(user , r.Name).Result
                }).ToList()
            };
         
            return View(userModel);
        }
 
        //  Edit --> Post
       [HttpPost]
       public async Task<IActionResult> Edit(UserRoleViewModel model)
       {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            // اللي عندي Roles هلف علي كل ال
            foreach (var role in model.Roles) // All Roles in the System
            {
                // ف كدا معناها انها ممنوعه User دي عند ال role.Name لو لقينا ال
                // user دي من عند ال role اقولو امسح ال userManager هكلم ال Check من عليها ولا لا عشان لو شال ال Check راح شال ال user وهشوف بردو ال 
                if (rolesForUser.Any( r => r == role.Name) && !role.IsSelected)
                       await _userManager.RemoveFromRoleAsync(user , role.Name);


                //  ده user  اللي بتدور عليها دلوقت لو ملقتهاش عندك دا معناه انها مش ممنوعه ل الrole لو ملقتش ان ال
                // دا user يعني علمت عليها لل select و ومكنتش عندي وانا جيت عملتلها
                if(!rolesForUser.Any( r => r == role.Name) &&  role.IsSelected)
                      await _userManager.AddToRoleAsync(user , role.Name);

            }
            return RedirectToAction(nameof(Index));

       }
    }
}
