using System;

namespace MyWeatherApp
{
    public class View
    {
        public void ShowWeather(IWeather weather)
        {
            if (weather is WeatherNow)
            {
                WeatherNow weatherNow = weather as WeatherNow;
                Console.WriteLine($"Date: {DateTime.Now.Date} \n City: {weatherNow.name} \n Weather: {weatherNow.weather[0].Description} \n" +
                                  $"Tempreature: {weatherNow.main.Temp} C \n Atmospheric pressure: {weatherNow.main.Pressure} \n Humidity: {weatherNow.main.Humidity} \n" +
                                  $"Wind speed: {weatherNow.wind.Speed} km/h");
                Console.WriteLine();
            }

            if (weather is WeatherForecast)
            {
                WeatherForecast weatherForecast = weather as WeatherForecast;
                Console.WriteLine($"City: {weatherForecast.name}");
                Console.WriteLine();
            }
        }
    }
}