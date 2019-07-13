using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Models
{
    public class FiTrackContext : IdentityDbContext
    {
        public FiTrackContext(DbContextOptions<FiTrackContext> options)
            : base(options)
        {
        }

        public DbSet<FiTrack.Models.Meal> Meal { get; set; }
        public DbSet<FiTrack.Models.Food> Food { get; set; }
        public DbSet<FiTrack.Models.Measure> Measure { get; set; }
        public DbSet<FiTrack.Models.Nutrient> Nutrient { get; set; }

        

    }
}
