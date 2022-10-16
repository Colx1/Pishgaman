using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pishgaman.Areas.Identity.Data;
using Pishgaman.Data;

[assembly: HostingStartup(typeof(Pishgaman.Areas.Identity.IdentityHostingStartup))]
namespace Pishgaman.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<PishgamanDB>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("PishgamanDBConnection")));


                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    options.User.RequireUniqueEmail = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 2;
                    options.Password.RequiredUniqueChars = 0;
                })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<PishgamanDB>();

                services.ConfigureApplicationCookie(x =>
                {
                    x.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = y => {
                            y.Response.Redirect("/Account/Login");
                            return Task.CompletedTask;
                        },

                        OnRedirectToAccessDenied = z =>
                        {
                            z.Response.Redirect("/AccessDenied");
                            return Task.CompletedTask;
                        }
                    };
                });

            });

        }
    }
}