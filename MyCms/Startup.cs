using DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCms.Areas;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ServiceLayer;

namespace MyCms
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<MyCmsContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IPageCommentRepository, PageCommentRepository>();
            services.AddControllersWithViews();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                options.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIED>();

            services.AddScoped<IMessageSender, MessageSender>();            
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "areas",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var role = new[] { "Owner"};
                foreach (var Newrole in role)
                {
                    if (!await rolemanager.RoleExistsAsync(Newrole))
                        await rolemanager.CreateAsync(new IdentityRole(Newrole));
                }
            }
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string username = "owner";
                string email = "owner@example.com";
                string password = "@Owner1234";
                if (await usermanager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = username;
                    user.Email = email;
                    user.EmailConfirmed = true;

                    await usermanager.CreateAsync(user, password);
                    await usermanager.AddToRoleAsync(user, "Owner");
                }
            }
        }
    }
}
