using FiTrack.Models.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiTrack.Models.ViewModels
{
    public class FoodNutrientViewModel
    {
        public List<FoodNutrientsItem> Foods { get; set; }

        public FoodNutrientViewModel()
        {
            Foods = new List<FoodNutrientsItem>();
        }

        public FoodNutrientViewModel(ICollection<Food> foods)
        {
            Foods = foods.ToList().ConvertAll(x => new FoodNutrientsItem(x));
        }
    }
}
