using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public class TimeLimitProxy : IModel
    {
        public readonly string _locationId;
        public readonly int _daysAhead;
        private Model _realModel;
        
        public TimeLimitProxy(string locationId, int daysAhead)
        {
            _locationId = locationId;
            _daysAhead = daysAhead;
            _realModel = new Model(_locationId, _daysAhead);
        }
        
        
        public IWeather GetWeather() //async?
        {
            //Checks if the last qiery to the weather API was not later than 10 min ago (that is the API requirement for free accounts)
            
            //Here will be showing of the cashed data if the queries limit is exceeded. 
            
            var currentTime = DateTime.Now;
            DateTime lastQueryTime;

            var t = ConfigurationManager.AppSettings.Get("LastQueryTime");
            if (DateTime.TryParse(t, out lastQueryTime))
            {
                var span = currentTime - lastQueryTime;
                if (span.TotalMinutes >= 10)
                {
                    Configuration currentConfig =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    currentConfig.AppSettings.Settings["LastQueryTime"].Value = currentTime.ToString();
                    currentConfig.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    return _realModel.GetWeather();
                }
                else
                {
                    int timeLeft = 10 - (int) span.TotalMinutes;
                    if (timeLeft == 1)
                    {
                        Console.WriteLine("Please wait for {0} minute before your next query.", timeLeft);
                    }
                    else Console.WriteLine("Please wait for {0} minutes before your next query.", timeLeft);
                    return null;
                }
            }

            Console.WriteLine("There is some problem with configuration file!");
            return null;
        }
    }
}