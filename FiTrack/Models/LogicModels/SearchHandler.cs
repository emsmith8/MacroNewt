using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FiTrack.Models.ViewModels;

namespace FiTrack.Models.LogicModels
{
    public class SearchHandler
    {


        /* Ideally will make this accept different API query types */

        public string CombineSearchTerms(string foodName)
        {
            string urlParameters = "?format=json&q=" + foodName + "&ds=Standard+Reference&sort=n&max=1500&offset=0&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";


            return urlParameters;
        }



        public List<QueryViewModel> StoreResults(String APIData)
        {

            List<Food> foodThingy = new List<Food>();

            JObject joResponse = JObject.Parse(APIData);
            JObject ojObject = (JObject)joResponse["list"];
            JArray array = (JArray)ojObject["item"];

            foodThingy = JsonConvert.DeserializeObject<List<Food>>(array.ToString());


            var blahBlah = new List<QueryViewModel>();
            
            foreach (var thingy in foodThingy)
            {
                blahBlah.Add(new QueryViewModel { Name = thingy.name, Ndbno = thingy.ndbno });
            }
            

/*
            int count = 0;

            foreach (var thingy in foodThingy)
            {
                Debug.WriteLine(count + " The name of this food is " + thingy.name + " and the ndbno is " + thingy.ndbno);
                count++;
            }

*/

            return blahBlah;

    //        ViewData["foodThingys"] = foodThingy;




        }


    }
}
