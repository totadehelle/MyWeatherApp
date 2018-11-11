using System;
using System.Linq;
using SQLitePCL;

namespace MyWeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var client = new LocationsContext())
            {
                client.Database.EnsureCreated();
            }
            
            
            
            LocationIdListBuilder builder = new LocationIdListBuilder(new LocationsContext());

            if (builder._context.Cities.Count() == 0)
            {
                builder.GetLocationIdDb();
            }
            

            //For testing only!
            string locationId;
            
            var cityId = from city in builder._context.Cities
                where city.Name == "Novinki"
                select city;

            if (cityId.Count() == 0)
            {
                Console.WriteLine("There is no such city in our database, please check if the spelling is correct.");
                return;
            }
                
            if (cityId.Count() > 1)
            {
                foreach (var city in cityId)
                {
                    //если при запуске приложения база уже существует, Coord получаемых объектов = null и здесь приложение падает. Если база создается после запуска, то все ок.
                    Console.WriteLine(
                        $"ID: {city.Id} \n City: {city.Name} \n Country: {city.Country} \n Coordinates: \n longitude: {city.Coord.lon}, \n latitude: {city.Coord.lat}");
                }

                Console.WriteLine("Please select your city and print its id:");
                locationId = Console.ReadLine();
                return;
            }

            locationId = cityId.First().Id.ToString();
            
            
            
            //Controller controller = new Controller(args);
        }
    }
}