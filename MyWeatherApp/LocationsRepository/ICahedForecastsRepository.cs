using System;
using System.Linq;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp.LocationsRepository
{
    public interface ICashedForecastsRepository : IDisposable
    {
        IQueryable<StoredWeather> Get(int locationID, int daysAhead, WeatherType type);
        StoredWeather Create(IWeather weather, string message, WeatherType type, int daysAhead);
        void Add(StoredWeather forecast);
    }
}