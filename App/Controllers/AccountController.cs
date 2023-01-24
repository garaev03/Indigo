using Indigo.Entities.Concrets;
using Indigo.Entities.DTOs.AppUserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol.Plugins;

namespace Indigo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (ModelState.IsValid)
            {
                AppUser NewUser = new()
                {
                    Email = register.Email,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    UserName = register.UserName,
                };
                IdentityResult result = await _userManager.CreateAsync(NewUser, register.Password);
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await _userManager.AddToRoleAsync(NewUser, "Admin");
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault().Description);
                    return View(register);
                }
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid) return View(login);
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "username or password incorrect");
                return View(login);
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password incorrect");
                return View(login);
            }

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
