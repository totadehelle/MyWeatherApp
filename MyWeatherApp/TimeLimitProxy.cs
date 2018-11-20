using System;
using System.Configuration;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public class TimeLimitProxy : IModel
    {
        private readonly string _locationId;
        private readonly int _daysAhead;
        private readonly Model _realModel;
        
        public TimeLimitProxy(string locationId, int daysAhead)
        {
            _locationId = locationId;
            _daysAhead = daysAhead;
            _realModel = new Model(_locationId, _daysAhead);
        }
        
        //Checks if the last query to the Weather API was not later than 10 min ago (that is the API requirement for free accounts)
        public IWeather GetWeather()
        {
            var currentTime = DateTime.Now;
            var time = ConfigurationManager.AppSettings.Get("LastQueryTime");
            if (DateTime.TryParse(time, out var lastQueryTime))
            {
                var span = currentTime - lastQueryTime;
                if (span.TotalMinutes >= 10)
                {
                    //saving new configuration
                    Configuration currentConfig =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    currentConfig.AppSettings.Settings["LastQueryTime"].Value = currentTime.ToString();
                    currentConfig.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    return _realModel.GetWeather();
                }

                int timeLeft = 10 - (int) span.TotalMinutes;
                if (timeLeft == 1)
                {
                    Console.WriteLine("Please wait for {0} minute before your next query.", timeLeft);
                }
                else Console.WriteLine("Please wait for {0} minutes before your next query.", timeLeft);
                return null;
            }

            Console.WriteLine("There is some problem with configuration file!");
            return null;
        }
    }
}