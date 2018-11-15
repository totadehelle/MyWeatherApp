using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWeatherApp.LocationsRepository
{
    public interface IRepository : IDisposable
    {
        IQueryable<City> GetCityList(string cityName);
    }
}