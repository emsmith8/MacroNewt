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
        public string Ndbno { get; set; }
        public string Nutrient_id { get; set; }
        public string Group { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public List<Measures> Measures { get; set; }
    }

    public class Measures
    {
        public string Label { get; set; }
        public string Eqv { get; set; }
        public string Eunit { get; set; }
        public string Qty { get; set; }
        public string Value { get; set; }
    }

}
