using FiTrack.Models.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiTrack.Models.ViewModels
{
    public class FoodViewModel
    {
        public List<FoodListItem> Foods {get;set;}

        public FoodViewModel()
        {
            Foods = new List<FoodListItem>();
        }

        public FoodViewModel(ICollection<Food> _foods)
        {
            Foods = _foods.ToList().ConvertAll(x => new FoodListItem(x));
        }
    }
}
