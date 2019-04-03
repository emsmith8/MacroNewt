using System.Diagnostics;

namespace FiTrack.Models.Type
{
    public class FoodListItem
    {
        public string Name { get; set; }
        public string Ndbno { get; set; }

        public FoodListItem()
        {
        }

        public FoodListItem(Food food)
        {
            Name = food.Name;
            Ndbno = food.Ndbno;
        }
    }
}
