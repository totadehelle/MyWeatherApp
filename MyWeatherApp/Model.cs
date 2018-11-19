using System;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public class Model : IModel
    {
        private const string APPID = "bbee93d67b25c3a25d873df876df5b23";

        private const string CURRENT_WEATHER_URI =
            "http://api.openweathermap.org/data/2.5/weather?units=metric&APPID=" + APPID + "&id=";

        private const string FORECAST_URI =
            "http://api.openweathermap.org/data/2.5/forecast?units=metric&APPID=" + APPID + "&id=";

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
            string path;
            WeatherType type;
            if (_daysAhead == 0)
            {
                path = CURRENT_WEATHER_URI + _locationId;
                type = WeatherType.Current;
            }
            else
            {
                path = FORECAST_URI + _locationId;
                type = WeatherType.Forecast;
            }

            return GetDataFromApi(path, type).Result;
        }



        private async Task<IWeather> GetDataFromApi(string path, WeatherType type)
        {
            try
            {
                client.BaseAddress = new Uri("http://localhost:64195/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                IWeather weather = null;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    switch (type)
                    {
                        case WeatherType.Current:
                            weather = await response.Content.ReadAsAsync<CurrentWeather>();
                            break;
                        case WeatherType.Forecast:
                            weather = await response.Content.ReadAsAsync<WeatherForecast>();
                            break;
                    }
                }

                return weather;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}