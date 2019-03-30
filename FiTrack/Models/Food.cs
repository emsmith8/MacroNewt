using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FiTrack.Models
{
    public class Food
    {
        [Key]
        public int FoodId { get; set; }
        public string Name { get; set; }
        public string ndbno { get; set; }
    }
}
