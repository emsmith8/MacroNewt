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

                string keyStuff = "User ID=esmithadmin;Password=" + context.Configuration["esmithadmin"];
                string connect1 = context.Configuration["ConnectionStrings:MacroNewtContext1:ConnectionString"];
                string connect2 = context.Configuration["ConnectionStrings:MacroNewtContext2:ConnectionString"];


                string fullConnex = connect1 + keyStuff + connect2;


                services.AddDbContext<MacroNewtContext>(options =>
                    options.UseSqlServer(
                        //context.Configuration.GetConnectionString("MacroNewtContext")));
                        fullConnex));

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