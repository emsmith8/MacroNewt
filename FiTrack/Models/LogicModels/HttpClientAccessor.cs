using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FiTrack.Models.LogicModels
{
    public static class HttpClientAccessor
    {
        public static Func<HttpClient> UsdaFactory = () =>
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("https://api.nal.usda.gov/ndb/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        };

        private static Lazy<HttpClient> client = new Lazy<HttpClient>(UsdaFactory);

        public static HttpClient HttpClient
        {
            get { return client.Value; }
        }

    }
}
