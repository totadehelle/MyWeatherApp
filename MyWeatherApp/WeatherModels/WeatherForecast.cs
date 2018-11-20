using System.Collections.Generic;
using MyWeatherApp.Repositories;

namespace MyWeatherApp.WeatherModels
{
    public class WeatherForecast : IWeather
    {
        public City City { get; set; }
        public List<CurrentWeather> List { get; set; }
    }
}