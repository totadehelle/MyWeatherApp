using System;
using System.Data.SQLite;
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
                    forecast.CityId == locationID
                where
                    forecast.QueryDate.Date == DateTime.Today
                where
                    forecast.RequiredDate.Date == DateTime.Today.AddDays(daysAhead)
                where
                    forecast.Type == type
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
                    forecast.CityId = currentWeather.id;
                    forecast.QueryDate = DateTime.Now;
                    forecast.RequiredDate = DateTime.Now;
                    forecast.Type = WeatherType.Current;
                    forecast.Message = message;
                    break;
                case WeatherType.Forecast:
                    WeatherForecast weatherForecast = weather as WeatherForecast;
                    forecast.CityId = weatherForecast.city.Id;
                    forecast.QueryDate = DateTime.Now;
                    forecast.RequiredDate = DateTime.Now.AddDays(daysAhead);
                    forecast.Type = WeatherType.Forecast;
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
                where forecast.QueryDate.Date != DateTime.Now.Date select forecast);
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