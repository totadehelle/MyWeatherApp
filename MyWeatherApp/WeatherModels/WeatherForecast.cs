using System.Collections.Generic;
using MyWeatherApp.LocationsRepository;

namespace MyWeatherApp.WeatherModels
{
    public class WeatherForecast : IWeather
    {
        public City city { get; set; }
        public List<CurrentWeather> list { get; set; }
    }
}