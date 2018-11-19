using System;

namespace MyWeatherApp.LocationsRepository
{
    public class UnitOfWork : IDisposable
    {
        private AppContext context = new AppContext();
        private SqliteCitiesRepository _citiesRepository;
        private SqliteCashedForecastsRepository _cashRepository;
 
        public SqliteCitiesRepository Cities
        {
            get
            {
                if (_citiesRepository == null)
                    _citiesRepository = new SqliteCitiesRepository(context);
                return _citiesRepository;
            }
        }
 
        public SqliteCashedForecastsRepository Cash
        {
            get
            {
                if (_cashRepository == null)
                    _cashRepository = new SqliteCashedForecastsRepository(context);
                return _cashRepository;
            }
        }
 
        public void Save()
        {
            context.SaveChanges();
        }
 
        private bool disposed = false;
 
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}