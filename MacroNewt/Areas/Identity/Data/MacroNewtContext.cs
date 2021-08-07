using MacroNewt.Application.Common.Interfaces;
using MacroNewt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MacroNewt.Areas.Identity.Data
{
    public class MacroNewtContext : IdentityContext, IMacroNewtDbContext
    {
        public MacroNewtContext(DbContextOptions<MacroNewtContext> options)
            : base(options)
        {
        }

        public DbSet<UserGoals> UserGoals { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<DailyCalTotal> DailyCalTotals { get; set; }


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
