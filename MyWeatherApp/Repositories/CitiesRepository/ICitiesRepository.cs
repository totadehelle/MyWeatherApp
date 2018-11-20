using System;
using System.Linq;

namespace MyWeatherApp.Repositories
{
    public interface ICitiesRepository : IDisposable
    {
        IQueryable<City> Get(string cityName);
        City GetFavourite();
        void SetFavourite(int id);
    }
}