using System.Collections.Generic;

namespace MyWeatherApp
{
    public class WeatherNow : IWeather
    {
        public string name { get; set; }
        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public float Speed { get; set; }
    }
}