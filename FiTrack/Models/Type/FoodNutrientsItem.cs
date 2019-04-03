using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiTrack.Models.Type
{
    public class FoodNutrientsItem
    {
        public string Nutrient_id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public List<Measures> Measures { get; set; }

        public FoodNutrientsItem()
        {
        }

        public FoodNutrientsItem(Food food)
        {
            Nutrient_id = food.Nutrient_id;
            Name = food.Name;
            Group = food.Group;
            Unit = food.Unit;
            Value = food.Value;
            Measures = food.Measures;
        }

    }
    
}
