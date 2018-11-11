using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyWeatherApp
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        
        public Coordinates Coord;
    }

    public class Coordinates
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }
    
    public class LocationsContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=cities.db");
        }
    }
    
    public class LocationIdListBuilder
    {
        private const string ResourceFilePath = @"/home/alter/RiderProjects/MyWeatherApp/MyWeatherApp/city.list.test.json";
        
        public readonly LocationsContext _context;

        public LocationIdListBuilder(LocationsContext context)
        {
            _context = context;
        }

        public void GetLocationIdDb()
        {
            List<City> citiesList = new List<City>();
            
            try
            {   
                using (StreamReader file = File.OpenText(ResourceFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    citiesList = (List<City>)serializer.Deserialize(file, typeof(List<City>));
                }
                
                AddCitiesToDb(citiesList);
                
            }
            
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        
        private void AddCitiesToDb(List<City> list)
        {
            try
            {
                
                    foreach (var city in list)
                    {
                        _context.Cities.Add(city);
                        _context.SaveChanges();
                    }
                
                Console.WriteLine("The database was successfully made!");
            }
            catch (Exception e)
            {
                Console.WriteLine("The database creation failed:");
                Console.WriteLine(e.Message);
            }
        }
    }
}