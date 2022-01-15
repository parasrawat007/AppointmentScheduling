using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModel;
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
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName=model.Email,
                    Email=model.Email,
                    Name=model.Name
                };
                var result = await _UserManger.CreateAsync(user);
                if(result.Succeeded)
                {
                    await _SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}
