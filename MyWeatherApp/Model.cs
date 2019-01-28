using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public class Model : IModel
    {
        private const string Appid = "bbee93d67b25c3a25d873df876df5b23";

        private const string CurrentWeatherUri =
            "http://api.openweathermap.org/data/2.5/weather?units=metric&APPID=" + Appid + "&id=";

        private const string ForecastUri =
            "http://api.openweathermap.org/data/2.5/forecast?units=metric&APPID=" + Appid + "&id=";

        private readonly string _locationId;
        private readonly int _daysAhead;

        private static readonly HttpClient Client = new HttpClient();

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
                path = CurrentWeatherUri + _locationId;
                type = WeatherType.Current;
            }
            else
            {
                path = ForecastUri + _locationId;
                type = WeatherType.Forecast;
            }

            return GetDataFromApi(path, type).Result;
        }

        private async Task<IWeather> GetDataFromApi(string path, WeatherType type)
        {
            try
            {
                Client.BaseAddress = new Uri("http://localhost:64195/");
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                IWeather weather = null;
                var response = await Client.GetAsync(path);
                if (!response.IsSuccessStatusCode) return null;
                switch (type)
                {
                    case WeatherType.Current:
                        weather = await response.Content.ReadAsAsync<CurrentWeather>();
                        break;
                    case WeatherType.Forecast:
                        weather = await response.Content.ReadAsAsync<WeatherForecast>();
                        break;
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