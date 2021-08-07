using MacroNewt.Areas.Identity.Data;
using MacroNewt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MacroNewt.Application.Common.Interfaces
{
    /// <summary>
    /// Interface that defines contract to a DBContext
    /// </summary>
    public interface IMacroNewtDbContext
    {
        DbSet<Meal> Meals { get; set; }
        DbSet<Food> Foods { get; set; }
        DbSet<Measure> Measures { get; set; }
        DbSet<Nutrient> Nutrients { get; set; }
        DbSet<DailyCalTotal> DailyCalTotals { get; set; }
        DbSet<UserGoals> UserGoals { get; set; }    
        DbSet<MacroNewtUser> Users { get;set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();

    }
}
