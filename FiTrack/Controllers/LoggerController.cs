using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FiTrack.Models;

namespace FiTrack.Controllers
{
    public class LoggerController : Controller
    {

        private const string URL = "https://api.nal.usda.gov/ndb/search/"; 
        
        public IActionResult Index()
        {
            return View();
        }
        
        public ActionResult SearchFoods(String foodName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            List<Food> foodThingy = new List<Food>();

            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string urlParameters = "?format=json&q=" + foodName + "&ds=Standard+Reference&sort=n&max=25&offset=0&api_key=5jOuzAkdWfOOH2x5yPgd2oWsyzGVkyrrkElAMSsl";

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                JObject joResponse = JObject.Parse(dataObjects);
                JObject ojObject = (JObject)joResponse["list"];
                JArray array = (JArray)ojObject["item"];
                
                foodThingy = JsonConvert.DeserializeObject<List<Food>>(array.ToString());

                foreach(var thingy in foodThingy)
                {
                    Debug.WriteLine("The name of this food is " + thingy.name + " and the ndbno is " + thingy.ndbno);
                }

                ViewData["foodThingys"] = foodThingy;

            } else
            {
                Debug.WriteLine("Something went wrong with the request I guess");
            }

            client.Dispose();

            return View();
        }
        
    }
}