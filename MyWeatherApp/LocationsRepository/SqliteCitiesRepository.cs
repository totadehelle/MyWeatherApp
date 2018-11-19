using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWeatherApp.LocationsRepository
{
    public class SqliteCitiesCitiesRepository : ICitiesRepository
    {
        private AppContext context;

        public SqliteCitiesCitiesRepository()
        {
            context = new AppContext();
        }
        
        public IQueryable<City> Get(string cityName)
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