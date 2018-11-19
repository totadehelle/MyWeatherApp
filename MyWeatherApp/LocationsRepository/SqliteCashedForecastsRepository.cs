using System;
using System.Linq;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp.LocationsRepository
{
    public class SqliteCashedForecastsRepository : ICashedForecastsRepository
    {
        private AppContext context;

        public SqliteCashedForecastsRepository()
        {
            context = new AppContext();
        }
        
        public IQueryable<StoredWeather> Get(int locationID, int daysAhead, WeatherType type)
        {
            DeleteObsoleteData();
            
            var forecastsFound = from forecast in context.CashedForecasts
                where
                    forecast.Id == locationID
                where
                    forecast.queryDate.Date == DateTime.Today
                where
                    forecast.requiredDate.Date == DateTime.Today.AddDays(daysAhead)
                where
                    forecast.type == type
                select forecast;
                    
            return forecastsFound;
        }

        public StoredWeather Create(IWeather weather, string message, WeatherType type, int daysAhead)
        {
            var forecast = new StoredWeather();
            switch (type)
            {
                case WeatherType.Current:
                    CurrentWeather currentWeather = weather as CurrentWeather;
                    forecast.Id = currentWeather.id;
                    forecast.queryDate = DateTime.Now;
                    forecast.requiredDate = DateTime.Now;
                    forecast.type = WeatherType.Current;
                    forecast.Message = message;
                    break;
                case WeatherType.Forecast:
                    WeatherForecast weatherForecast = weather as WeatherForecast;
                    forecast.Id = weatherForecast.city.Id;
                    forecast.queryDate = DateTime.Now;
                    forecast.requiredDate = DateTime.Now.AddDays(daysAhead);
                    forecast.type = WeatherType.Forecast;
                    forecast.Message = message;
                    break;
            }

            return forecast;
        }

        public void Add(StoredWeather forecast)
        {
            context.CashedForecasts.Add(forecast);
            context.SaveChanges();
        }

        private void DeleteObsoleteData()
        {
            context.CashedForecasts.RemoveRange(from forecast in context.CashedForecasts 
                where forecast.queryDate.Date != DateTime.Now.Date select forecast);
            context.SaveChanges();
        }

        private bool disposed = false;
 
        public virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}