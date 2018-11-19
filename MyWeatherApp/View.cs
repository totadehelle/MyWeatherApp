using System;
using System.Text;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public class View
    {
        public void ShowWeather(IWeather weather)
        {
            if (weather is CurrentWeather)
            {
                CurrentWeather currentWeather = weather as CurrentWeather;
                Console.WriteLine($"Date: {DateTime.Now} \n City: {currentWeather.name} \n Weather: {currentWeather.weather[0].Description} \n" +
                                  $"Temperature: {currentWeather.main.Temp} C \n Atmospheric pressure: {currentWeather.main.Pressure} \n Humidity: {currentWeather.main.Humidity} \n" +
                                  $"Wind speed: {currentWeather.wind.Speed} km/h");
            }

            if (weather is WeatherForecast)
            {
                WeatherForecast weatherForecast = weather as WeatherForecast;
                Console.WriteLine($"City: {weatherForecast.city.Name}");
            }
        }

        public string ShowCurrentWeather(CurrentWeather currentWeather)
        {
            string message =
                $"Date: {DateTime.Now} \n City: {currentWeather.name} \n Weather: {currentWeather.weather[0].Description} \n" +
                $"Tempreature: {currentWeather.main.Temp} C \n Atmospheric pressure: {currentWeather.main.Pressure} \n Humidity: {currentWeather.main.Humidity} \n" +
                $"Wind speed: {currentWeather.wind.Speed} km/h";
            Console.WriteLine(message);
            return message;
        }

        public string ShowWeatherForecast(WeatherForecast weatherForecast)
        {
            StringBuilder message = new StringBuilder($"City: {weatherForecast.city.Name} \n");
            
            foreach (var partOfDay in weatherForecast.list)
            {
                message.Append($"{partOfDay.Date} \n Weather: {partOfDay.weather[0].Description} \n" +
                               $"Temperature: {partOfDay.main.Temp} C \n Atmospheric pressure: {partOfDay.main.Pressure} \n Humidity: {partOfDay.main.Humidity} \n" +
                               $"Wind speed: {partOfDay.wind.Speed} km/h \n");
            }

            Console.WriteLine(message);
            return message.ToString();
        }
    }
}