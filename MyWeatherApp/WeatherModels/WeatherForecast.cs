using System.Collections.Generic;
using MyWeatherApp.Repositories;

namespace MyWeatherApp.WeatherModels
{
    public class WeatherForecast : IWeather
    {
        public City city { get; set; }
        public List<CurrentWeather> list { get; set; }
    }
}