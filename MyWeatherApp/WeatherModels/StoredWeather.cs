using System;

namespace MyWeatherApp.WeatherModels
{
    public class StoredWeather
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime QueryDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public WeatherType Type { get; set; }
        public string Message { get; set; }
    }
}