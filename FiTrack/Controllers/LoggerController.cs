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

        //&fg=Dairy+and+Egg+Products

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult SearchFoods(String foodName)
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
                //          Debug.WriteLine("The tostringed jarray is " + array.ToString());
                //               var dataObjects = response.Content.ReadAsStringAsync().Result;
                //            var dataObjects = response.Content.ReadAsStringAsync().Result;
                //              Debug.WriteLine("Your food results are " + dataObjects);
                
                foodThingy = JsonConvert.DeserializeObject<List<Food>>(array.ToString());

                foreach(var thingy in foodThingy)
                {
                    Debug.WriteLine("The name of this food is " + thingy.name + " and the ndbno is " + thingy.ndbno);
                }

                ViewData["foodThingys"] = foodThingy;

        //        Debug.WriteLine("The foodThingy tostring is " + foodThingy[0].name);

                //               JObject joResponse = JObject.Parse(dataObjects);
                //               JObject ojObject = (JObject)joResponse["list"];
                //               JArray array = (JArray)ojObject["item"];
                //               Debug.WriteLine("The array[3] thingy is " + array[3].ToString());
                //               int ndb = Convert.ToInt32(array[3].ToString());

                //               Debug.WriteLine("The NDB is " + ndb);

            } else
            {
                Debug.WriteLine("Something went wrong with the request I guess");
            }

            client.Dispose();

            return View();
        }

        /*
                public ActionResult storeFoods()
                {

                }
          */
    }
}