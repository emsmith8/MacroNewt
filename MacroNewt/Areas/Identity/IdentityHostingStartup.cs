using System;
using MacroNewt.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MacroNewt.Areas.Identity.Data.IdentityHostingStartup))]
namespace MacroNewt.Areas.Identity.Data
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDbContext<MacroNewtContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MacroNewtContext")));

                services.AddIdentity<MacroNewtUser, IdentityRole>()
                   // .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<MacroNewtContext>()
                    .AddDefaultTokenProviders();

             //   services.AddDefaultIdentity<MacroNewtUser>()
             //       .AddEntityFrameworkStores<MacroNewtContext>();
            });
        }

    }
}