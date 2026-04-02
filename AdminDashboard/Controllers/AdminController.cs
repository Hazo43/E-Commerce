using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.IdentityModule;

namespace AdminDashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            // DB م موجود عندي في ال user لو ال
            if ( user is null)
            {
                ModelState.AddModelError("", "Invalid Login attempt");
                return View(loginDTO);
            }
            // عشان نشوف هو دخلو صح ولا لا password هنجيب ال
            var password = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);

            //  (Admin Or SuperAdmin) ل role اللي انت دخلتو غلط او انت معندكش password لو ال
            if (!password.Succeeded || (!await _userManager.IsInRoleAsync(user, "Admin") && (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))))
            {
                ModelState.AddModelError("", "You are not authoruzed");
                return View(loginDTO);
            }

            // (Admin Or SuperAdmin) ل Role صح و عندو password لو هو مدخل ال
            // Home بتاع ال Controller في ال Index هرجعو علي ال
            return RedirectToAction(nameof(Index) , "Home");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
