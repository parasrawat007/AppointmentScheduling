using AppointmentScheduling.Models;
using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options=>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddControllersWithViews();
            services.AddTransient<IAppointmentService, AppointmentService>();//For User Service
            services.AddHttpContextAccessor();//For Identity
            services.AddDistributedMemoryCache(); //For Session
            services.AddSession(options=> {
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });//For Session
            services.AddScoped<IEmailSender, EmailSender>(); //MailJet
            services.ConfigureApplicationCookie(option => {
                option.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/AccessDenied");
            });
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();//For Identity
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();   //For Static Resouces

            app.UseRouting();      //for Routing

            app.UseAuthentication(); 

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
