namespace FiTrack.Models.Type
{
    public class FoodListItem
    {
        public string Name { get; set; }
        public string Ndbno { get; set; }

        public FoodListItem()
        {
        }

        public FoodListItem(Food _food)
        {
            Name = _food.Name;
            Ndbno = _food.ndbno;
        }
    }
}
