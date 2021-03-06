﻿using MacroNewt.Areas.Identity.Data;
using MacroNewt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MacroNewt
{
    public class Startup
    {
        public UserManager<MacroNewtUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;

        public Startup(IConfiguration configuration, UserManager<MacroNewtUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            Configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                //options.SignIn.RequireConfirmedEmail = true; /* allow user to still sign in but not log meals */
            });

            services.AddDbContext<MacroNewtContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MacroNewtContext")));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Admin"));
                //options.AddPolicy("AllUsers", policy => policy.RequireRole("User"));
            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });



            services.AddSingleton<IEmailSender, EmailSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UsePathBase("/macronewt");

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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            var serviceProvider = app.ApplicationServices;

            CreateRoles(serviceProvider).Wait();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }


        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            string[] roleNames = { "Admin", "User" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var powerUser = new MacroNewtUser
            {
                UserName = Configuration["MacroNewtUserName"],
                Email = Configuration["MacroNewtUserEmail"],
                Name = "Admin",
                ProfileName = "Admin"
            };

            string userPWD = Configuration["MacroNewtPassword"];
            var _user = await _userManager.FindByEmailAsync(Configuration["MacroNewtUserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await _userManager.CreateAsync(powerUser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }

        }
    }
}
