namespace MyWeatherApp
{
    public class Forecast
    {
        public string name { get; set; }
        public Weather weather { get; set; }
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