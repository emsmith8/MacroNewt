using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiTrack.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public String Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime MealDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime MealTime { get; set; }

        public List<Food> FoodComponents { get; set; }

        public int Calories { get; set; }

    }
}
