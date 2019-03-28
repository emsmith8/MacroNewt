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

            var blahBlah = new List<QueryViewModel>();

            JObject joResponse = JObject.Parse(APIData);
            JObject ojObject = (JObject)joResponse["list"];
            JArray array = (JArray)ojObject["item"];

            blahBlah = JsonConvert.DeserializeObject<List<QueryViewModel>>(array.ToString());
  
            return blahBlah;
            
        }


    }
}
