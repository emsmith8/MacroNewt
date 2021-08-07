using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Areas.Identity.Data
{
    public class IdentityContext : IdentityDbContext<MacroNewtUser>
    {
        public IdentityContext(DbContextOptions options) : base(options) { }
    }
}
