using System.Collections.Generic;
using System.Linq;

namespace MyWeatherApp.LocationsRepository
{
    interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        void Create();
    }
}