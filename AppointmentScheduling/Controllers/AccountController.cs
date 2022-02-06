using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModel;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _UserManger;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> UManger,
            SignInManager<ApplicationUser> SManager, RoleManager<IdentityRole> RManager)
        {
            _db = db;
            _UserManger = UManger;
            _SignInManager = SManager;
            _RoleManager = RManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _UserManger.FindByNameAsync(model.Email);
                    HttpContext.Session.SetString("SSUserName", user.Name); 
                   //var UserName=HttpContext.Session.GetString("SSUserName"); Retriving in Controller 
                    return RedirectToAction("Index", "Appointment");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Register()
        {
            if (!_RoleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _RoleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _RoleManager.CreateAsync(new IdentityRole(Helper.Patient));
                await _RoleManager.CreateAsync(new IdentityRole(Helper.Doctor));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //   admin@app.com   - Admin@123           

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };
                var result = await _UserManger.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _UserManger.AddToRoleAsync(user, model.RoleName);
                    await _SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
