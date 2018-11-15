using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace MyWeatherApp
{
    public class LimitProxy : IModel
    {
        public readonly string _locationId;
        public readonly int _daysAhead;
        private Model _realModel;
        
        public LimitProxy(string locationId, int daysAhead)
        {
            _locationId = locationId;
            _daysAhead = daysAhead;
            _realModel = new Model(_locationId, _daysAhead);
        }
        
        
        public Task<WeatherNow> GetWeatherNow()
        {
            //проверка что последний запрос был более чем 10 мин назад
            var currentTime = DateTime.Now;
            DateTime lastQueryTime;

            if (DateTime.TryParse(ConfigurationManager.AppSettings.Get("LastQueryTime"), out lastQueryTime))
            {
                var span = currentTime - lastQueryTime;
                if (span.TotalMinutes > 10)
                {
                    Configuration currentConfig =
                        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    currentConfig.AppSettings.Settings["LastQueryTime"].Value = currentTime.ToString();
                    currentConfig.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    return _realModel.GetWeatherNow();
                }
                else
                {
                    int timeLeft = 10 - (int) span.TotalMinutes;
                    Console.WriteLine("Please wait for {0} minutes before your next query.", timeLeft);
                    return null;
                }
            }

            Console.WriteLine("There is some problem with configuration file!");
            return null;
        }
    }
}