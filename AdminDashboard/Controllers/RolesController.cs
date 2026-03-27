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
        // Index
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // Create
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            //1- ميسبوش فاضي name متاحه ب الشروط اللي ان حاطتها متكونش اكتر من256 حرف و يبعت ال ModelState في حاله ال
            if (ModelState.IsValid)
            {
                // 1.1- الاول Role بنجيب ال
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);

                //1.2- Role مش موجوده هضيف ال Role لو ال
                if (!roleExist)
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

        // Edit -->  [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            // Index و هرجعو ع صفحه ال Error مش موجوده هرجعلو role لو ال
            if (role == null)
            {
                ModelState.AddModelError("Id", "No Role Exist With This Id.");
                return RedirectToAction(nameof(Index));
            }

            // Edit موجوده بقا هروح اعدل اعليها واعملها  role لو   
            // updateRoleViewModel  بتاع ال Name  يساوي الName و هخلي ال id اخليه يساوي ال id هروح اضيف ال
            UpdateRoleViewModel updateRoleViewModel = new UpdateRoleViewModel
            {
                Id = id,
                Name = role.Name!
            };

            return View(updateRoleViewModel);
        }

        // Edit -->  [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model)
        {
            // ميسبوش فاضي name متاحه ب الشروط اللي ان حاطتها متكونش اكتر من256 حرف و يبعت ال ModelState في حاله ال
            if (ModelState.IsValid)
            {
                // الاول Role بنجيب ال
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);

                // update هعملها Role مش موجوده هعدل ال Role لو ال
                if (!roleExist)
                {
                    // الاول roleId هروح اجيب ال
                    var role = await _roleManager.FindByIdAsync(model.Id);

                    // update عشان نعملها form  اللي اتبعت في ال Name بنفس ال update لو هيه موجود هعملها
                    if (role is not null)
                    {
                        role.Name = model.Name;
                        await _roleManager.UpdateAsync(role); 
                    }
                    // Index اللي في ال Roles  بعد ما تضيف و تخلص روح اعرضلي كل 
                    return RedirectToAction(nameof(Index));
                }

                //  View(model)   و رجعو ع ال Error موجوده قبل كدا روح رجعلو roleExist لو بقا ال
                else
                {
                    // Error كانت موجوده بنفس الاسم قبل كدا روح رجعلو role لو ال
                    ModelState.AddModelError("Name", " Name Already Exists");
                    return View(model);
                }

            }


            // IsValid مش ModelState لو بقا ال
            // بعد ما تخلص كل حاجه Index وفي الاخر خالص روح لرجعو ع ال
            return RedirectToAction(nameof(Index));


        }

        // Delete
        public async Task<ActionResult> Delete(string id)
        {
            //1- دي role بتاع ال id هنجيب ال
            var role = await _roleManager.FindByIdAsync(id);
           
            //2- هنشوف موجود ولا لا لو موجود احذفو
            if (role is not null)
                await _roleManager.DeleteAsync(role);
            
            //3- Index لو مش موجود رجعو ع صفحه ال
                ModelState.AddModelError("Id", "No Role Exist With This Id.");
                return RedirectToAction(nameof(Index));
            
        }
    }
}
