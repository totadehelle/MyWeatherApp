using System;
using System.Text;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public class View
    {
        public string ShowWeather(IWeather weather, WeatherType type)
        {
            switch (type)
            {
                case WeatherType.Current:
                    return ShowCurrentWeather(weather as CurrentWeather);

                case WeatherType.Forecast:
                    return ShowWeatherForecast(weather as WeatherForecast);
            }

            return null;
        }

        private string ShowCurrentWeather(CurrentWeather currentWeather)
        {
            string message =
                $"\nDate: {DateTime.Now} \n City: {currentWeather.name} \n Weather: {currentWeather.weather[0].Description} \n" +
                $"Tempreature: {currentWeather.main.Temp} C \n Atmospheric pressure: {currentWeather.main.Pressure} \n Humidity: {currentWeather.main.Humidity} \n" +
                $"Wind speed: {currentWeather.wind.Speed} km/h";
            Console.WriteLine(message);
            return message;
        }

        private string ShowWeatherForecast(WeatherForecast weatherForecast)
        {
            StringBuilder message = new StringBuilder($"City: {weatherForecast.city.Name} \n");
            
            foreach (var partOfDay in weatherForecast.list)
            {
                message.Append($"\n\n{partOfDay.Date} \n Weather: {partOfDay.weather[0].Description} \n" +
                               $"Temperature: {partOfDay.main.Temp} C \n Atmospheric pressure: {partOfDay.main.Pressure} \n Humidity: {partOfDay.main.Humidity} \n" +
                               $"Wind speed: {partOfDay.wind.Speed} km/h");
            }

            Console.WriteLine(message);
            return message.ToString();
        }
    }
}