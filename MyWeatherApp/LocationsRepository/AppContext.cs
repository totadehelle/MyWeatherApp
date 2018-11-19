using System;
using Microsoft.EntityFrameworkCore;
using MyWeatherApp.WeatherModels;

namespace MyWeatherApp.LocationsRepository

{
    public class AppContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<StoredWeather> CashedForecasts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=weatherapp.db");
        }
    }
}