using System;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyWeatherApp
{
    public class Model : IModel
    {
        private const string APPID = "bbee93d67b25c3a25d873df876df5b23";
        private const string URI = "api.openweathermap.org/data/2.5/weather?units=metric&APPID=" + APPID + "&id=";
       
        private string _locationId;
        private int _daysAhead;
        
        static HttpClient client = new HttpClient();
        
        public Model(string locationId, int daysAhead)
        {
            _locationId = locationId;
            //_daysAhead = daysAhead;
        }

        public async Task<WeatherNow> GetWeatherNow()
        {
            try
            {
                client.BaseAddress = new Uri("http://localhost:64195/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            
                WeatherNow weatherNow = null;
                Uri path = new Uri(URI + _locationId);
            
                HttpResponseMessage response = await client.GetAsync(path.PathAndQuery);
                if (response.IsSuccessStatusCode)
                {
                    weatherNow = await response.Content.ReadAsAsync<WeatherNow>();
                }
            
                return weatherNow;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;

        }
    }
}