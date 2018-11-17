using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWeatherApp.LocationsRepository
{
    public interface ICitiesRepository : IDisposable
    {
        IQueryable<City> Get(string cityName);
    }
}