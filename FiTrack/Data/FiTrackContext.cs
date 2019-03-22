using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Models
{
    public class FiTrackContext : DbContext
    {
        public FiTrackContext (DbContextOptions<FiTrackContext> options)
            : base(options)
        {
        }

        public DbSet<FiTrack.Models.Meal> Meal { get; set; }
    }
}
