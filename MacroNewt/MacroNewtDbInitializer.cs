using MacroNewt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MacroNewt
{
    public class MacroNewtDbInitializer
    {
        public MacroNewtContext _context;
        private readonly UserManager<MacroNewtUser> _userManager;

        public MacroNewtDbInitializer(MacroNewtContext context, UserManager<MacroNewtUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void SeedAdmin()
        {

            if (_userManager.FindByEmailAsync("evan.matthew.smith@gmail.com").Result == null)
            {
                MacroNewtUser user = new MacroNewtUser
                {
                    Name = "Evan",
                    ProfileName = "Evan",
                    UserName = "evan.matthew.smith@gmail.com",
                    Email = "evan.matthew.smith@gmail.com"
                };

                IdentityResult result = _userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                }
            }
        }

    }
}
