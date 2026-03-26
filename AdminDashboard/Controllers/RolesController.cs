using AdminDashboard.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Create(RoleViewModel model)
        {
            //1- ميسبوش فاضي name متاحه ب الشروط اللي ان حاطتها متكونش اكتر من256 حرف و يبعت ال ModelState في حاله ال
            if ( ModelState.IsValid )
            {
               // 1.1- الاول Role بنجيب ال
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);

                //1.2- Role مش موجوده هضيف ال Role لو ال
                if ( !roleExist )
                {
                    // اللي هو عاوز الاسم بتاعها IdentityRole علي حسب ال Role هضيف 
                    await _roleManager.CreateAsync(new IdentityRole(model.Name));
                   
                    // Index اللي في ال Roles  بعد ما تضيف و تخلص روح اعرضلي كل الRole
                    return RedirectToAction(nameof(Index));
                }
                //1.3- موجوده role ان ال Error موجوده هروح ارجعلو roleExist لو بقا ال

                ModelState.AddModelError("Name", " Name Already Exists");
            }

            //2- name more than 256 char او بعت name في حاله انو مبعتش ال Index هرجعو ع ال
            //      اللي كانت موجوده قبل كدا  Roles و رجعو ع ال
            return View(nameof(Index), await _roleManager.Roles.ToListAsync());
        }
    }
}
