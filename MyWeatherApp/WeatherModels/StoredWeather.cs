using System;

namespace MyWeatherApp.WeatherModels
{
    public class StoredWeather
    {
        public int Id { get; set; }
        public DateTime queryDate { get; set; }
        public DateTime requiredDate { get; set; }
        public WeatherType type { get; set; }
        public string Message { get; set; }
    }
}