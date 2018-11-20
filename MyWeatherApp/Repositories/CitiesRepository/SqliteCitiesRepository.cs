using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyWeatherApp.Repositories
{
    public class SqliteCitiesRepository : ICitiesRepository
    {
        private AppContext _context;

        public SqliteCitiesRepository()
        {
            _context = new AppContext();
        }
        
        public IQueryable<City> Get(string cityName)
        {
            var citiesFound = from city in _context.Cities
                where city.Name == cityName
                select city;

            return citiesFound;
        }

        public City GetFavourite()
        {
            return _context.Cities.FirstOrDefault(c => c.IsPreferred == true);
        }

        public void SetFavourite(int id)
        {
            var exFavourite = _context.Cities.FirstOrDefault(c => c.IsPreferred == true);
            
            if (exFavourite != null)
            {
                if (exFavourite.Id == id) return;
                exFavourite.IsPreferred = false;
                _context.Entry(exFavourite).State = EntityState.Modified;
            }
            
            var newFavourite = _context.Cities.First(c => c.Id == id);
            newFavourite.IsPreferred = true;
            _context.Entry(newFavourite).State = EntityState.Modified;
            
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    _context.Dispose();
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