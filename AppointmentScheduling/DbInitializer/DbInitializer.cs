using AppointmentScheduling.Models;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> UManager, RoleManager<IdentityRole> RManager)
        {
            _db = db;
            _UserManager = UManager;
            _RoleManager = RManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

               
            }
            if (_db.Roles.Any(x => x.Name == Helper.Admin)) return;
          
            _RoleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _RoleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();
            _RoleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();

            _UserManager.CreateAsync(new ApplicationUser { 
                Name="Admin",
                Email="admin@app.com",
                UserName="admin@app.com",
                EmailConfirmed=true,
            },"Admin@123").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@app.com");
            _UserManager.AddToRoleAsync(user, Helper.Admin);
        }
    }
}
