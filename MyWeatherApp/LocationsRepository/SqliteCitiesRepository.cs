using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWeatherApp.LocationsRepository
{
    public class SqliteCitiesRepository : IRepository
    {
        private LocationsContext context;

        public SqliteCitiesRepository()
        {
            context = new LocationsContext();
        }
        
        public IQueryable<City> GetCityList(string cityName)
        {
            var citiesFound = from city in context.Cities
                where city.Name == cityName
                select city;

            return citiesFound;
            
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