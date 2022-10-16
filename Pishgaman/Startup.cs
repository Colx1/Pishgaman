using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Pishgaman.Areas.Identity.Data;
using Pishgaman.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Pishgaman.JwtHelper;
using Pishgaman.Middleware;
using Pishgaman.Data;

namespace Pishgaman
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddControllersWithViews();

            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient(typeof(DBRepository<,,>));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddCookie(opt => opt.SlidingExpiration = true)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromDays(1),
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<PishgamanDB>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<MiddlewareHandler>();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            InitAccounts(serviceProvider).Wait();
        }

        private async Task InitAccounts(IServiceProvider serviceProvider)
        {

            var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var usermaneger = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = new List<string> { "Read", "Write" };

            var accounts = new List<string> { "Account1", "Account2" };

            foreach (var RoleName in roles)
            {
                if (await rolemanager.RoleExistsAsync(RoleName) == false)
                {
                    var role = new IdentityRole(RoleName);
                    await rolemanager.CreateAsync(role);
                }
            }

            foreach (var accName in accounts)
            {
                var tempuser = await usermaneger.FindByNameAsync(Configuration[$"AccountSamples:{accName}:Username"]);
                if (tempuser == null)
                {
                    tempuser = new ApplicationUser
                    {
                        UserName = Configuration[$"AccountSamples:{accName}:Username"],
                    };
                    usermaneger.CreateAsync(tempuser, Configuration[$"AccountSamples:{accName}:Password"]).Wait();
                }

                switch (Configuration[$"AccountSamples:{accName}:Username"])
                {
                    case "Read":
                        if (!await usermaneger.IsInRoleAsync(tempuser, "Read"))
                            usermaneger.AddToRoleAsync(tempuser, "Read").Wait();
                        break;

                    case "ReadWrite":
                        if (!await usermaneger.IsInRoleAsync(tempuser, "Read") 
                            || !await usermaneger.IsInRoleAsync(tempuser, "Write"))
                        {
                            usermaneger.AddToRoleAsync(tempuser, "Read").Wait();
                            usermaneger.AddToRoleAsync(tempuser, "Write").Wait();
                        }
                        break;

                    default:
                        break;
                }

            }

        }
    }
}
