using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FiTrack.Models.ViewModels;
using FiTrack.Models.Type;
using System.Net.Http;

namespace FiTrack.Models.LogicModels
{
    public class SearchHandler
    {

        public string OrganizeSearchQ(string foodName)
        {
            string urlParameters = "search/?format=json&q=" + foodName + "&ds=Standard+Reference&sort=n&max=1500&offset=0&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";

            return urlParameters;
        }


        public string OrganizeReportQ(string foodNdbno)
        {
            string urlParameters = "reports/?format=json&ndbno=" + foodNdbno + "&type=b&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";
            
            return urlParameters;
        }


        public FoodViewModel StoreSearchReturns(String APIData)
        {
            JObject joResponse = JObject.Parse(APIData);
            JObject ojObject = (JObject)joResponse["list"];
            JArray array = (JArray)ojObject["item"];
            
            List<FoodListItem> availableFoods = JsonConvert.DeserializeObject<List<FoodListItem>>(array.ToString());

            FoodViewModel fvm = new FoodViewModel()
            {
                Foods = availableFoods
            };

            return fvm;
        }


        public FoodNutrientViewModel StoreReportReturns(String APIData, List<FoodListItem> foodList)
        {
 /*           foreach (String x in APIData)
            {
                JObject joResponse = JObject.Parse(x);

                JObject ojObject = (JObject)joResponse["report"];
                JArray array = (JArray)ojObject["food"]["nutrients"];
            }
   */         JObject joResponse = JObject.Parse(APIData);

            JObject ojObject = (JObject)joResponse["report"];
            JArray array = (JArray)ojObject["food"]["nutrients"];


            List<FoodNutrientsItem> foodNutrients = JsonConvert.DeserializeObject<List<FoodNutrientsItem>>(array.ToString());
            //       List<FoodNutrientsItem> foodNutrients = JsonConvert.DeserializeObject<List<FoodNutrientsItem>>(array.ToString());

            FoodNutrientViewModel fnvm = new FoodNutrientViewModel()
            {
                Foods = foodNutrients
            };
            

            return fnvm;
        }


    }
}
