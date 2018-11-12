using System;
using Microsoft.EntityFrameworkCore;

namespace MyWeatherApp.LocationsRepository

{
    public class LocationsContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=cities.db");
        }
    }
}