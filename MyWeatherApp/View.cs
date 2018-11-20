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
                $"\nDate: {DateTime.Now} \nCity: {currentWeather.Name} \nWeather: {currentWeather.Weather[0].Description} \n" +
                $"Temperature: {currentWeather.Main.Temp} C \nAtmospheric pressure: {currentWeather.Main.Pressure} hPa \nHumidity: {currentWeather.Main.Humidity} % \n" +
                $"Wind speed: {currentWeather.Wind.Speed} meter/sec";
            Console.WriteLine(message);
            return message;
        }

        private string ShowWeatherForecast(WeatherForecast weatherForecast)
        {
            StringBuilder message = new StringBuilder($"City: {weatherForecast.City.Name} \n");
            
            foreach (var partOfDay in weatherForecast.List)
            {
                message.Append($"\n\n{partOfDay.Date} \nWeather: {partOfDay.Weather[0].Description} \n" +
                               $"Temperature: {partOfDay.Main.Temp} C \nAtmospheric pressure: {partOfDay.Main.Pressure} hPa \nHumidity: {partOfDay.Main.Humidity} % \n" +
                               $"Wind speed: {partOfDay.Wind.Speed} meter/sec");
            }

            Console.WriteLine(message);
            return message.ToString();
        }
    }
}