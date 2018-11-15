using System;
using System.Collections.Generic;

namespace MyWeatherApp.LocationsRepository
{
    public interface IRepository : IDisposable
    {
        IEnumerable<City> GetCityList(string cityName);
    }
}