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
        private const string URI = "http://api.openweathermap.org/data/2.5/weather?units=metric&APPID=" + APPID + "&id=";
       
        private string _locationId;
        private int _daysAhead;
        
        static HttpClient client = new HttpClient();
        
        public Model(string locationId, int daysAhead)
        {
            _locationId = locationId;
            _daysAhead = daysAhead;
        }

        public IWeather GetWeather()
        {
            if (_daysAhead == 0)
            {
                return GetWeatherNow().Result;
            }
            
            return GetWeatherForecast(_daysAhead).Result;
        }
        
        private async Task<WeatherNow> GetWeatherNow()
        {
            try
            {
                client.BaseAddress = new Uri("http://localhost:64195/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            
                WeatherNow weatherNow = null;
                var path = URI + _locationId;

                /*Bug: Cannot deserialize the current JSON array (e.g. [1,2,3]) into type 'MyWeatherApp.Weather' because the type requires a JSON object (e.g. {"name":"value"}) to deserialize correctly. To fix this error either change the JSON to a JSON object (e.g. {"name":"value"}) or change the deserialized type to an array or a type that implements a collection interface (e.g. ICollection, IList) like List<T> that can be deserialized from a JSON array. JsonArrayAttribute can also be added to the type to force it to deserialize from a JSON array. Path 'weather', line 1, position 46.*/
                
                HttpResponseMessage response = await client.GetAsync(path);
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
        
        private async Task<WeatherNow> GetWeatherForecast(int daysAhead)
        {
            //there will be getting the forecast

            return null;
        }
    }
}