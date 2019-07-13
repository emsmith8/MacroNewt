using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MacroNewt.Areas.Identity.Data
{
    public class MacroNewtContext : IdentityDbContext<MacroNewtUser>
    {
        public MacroNewtContext(DbContextOptions<MacroNewtContext> options)
            : base(options)
        {
        }

        public DbSet<MacroNewt.Models.Meal> Meal { get; set; }
        public DbSet<MacroNewt.Models.Food> Food { get; set; }
        public DbSet<MacroNewt.Models.Measure> Measure { get; set; }
        public DbSet<MacroNewt.Models.Nutrient> Nutrient { get; set; }
        public DbSet<MacroNewt.Areas.Identity.Data.DailyCalTotal> DailyCalTotal { get; set; }
        public DbSet<MacroNewt.Areas.Identity.Data.UserGoals> UserGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MacroNewtUser>()
                .HasOne(x => x.Goals)
                .WithOne(y => y.MacroNewtUser)
                .HasForeignKey<UserGoals>(z => z.Id);

            builder.Entity<MacroNewtUser>()
                .HasMany(x => x.DailyCalTotals)
                .WithOne(y => y.MacroNewtUser)
                .HasForeignKey(z => z.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
