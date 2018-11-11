using System;
using System.Linq;
using MyWeatherApp.LocationsRepository;

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
            
            CitiesDbBuilder builder = new CitiesDbBuilder(new LocationsContext());
            
            builder.DecompressSourceFile(builder.DownloadSourceFile());
            return;

            if (builder._context.Cities.Count() == 0)
            {
                builder.MakeCitiesDbFromJson();
            }
            

            //For testing only!
            string locationId;
            
            var citiesFound = from city in builder._context.Cities
                where city.Name == "Novinki"
                select city;

            if (citiesFound.Count() == 0)
            {
                Console.WriteLine("The city is not found.");
                return;
            }
                
            if (citiesFound.Count() > 1)
            {
                foreach (var city in citiesFound)
                {
                    //если при запуске приложения база уже существует, Coord получаемых объектов = null и здесь приложение падает. Если база создается после запуска, то все ок.
                    Console.WriteLine(
                        $"ID: {city.Id} \n City: {city.Name} \n Country: {city.Country} \n Coordinates: \n longitude: {city.Coord.lon}, \n latitude: {city.Coord.lat} \n");
                }

                Console.WriteLine("Please select your city and print its id:");
                locationId = Console.ReadLine();
                return;
            }

            locationId = citiesFound.First().Id.ToString();
            
            
            
            //Controller controller = new Controller(args);
        }
    }
}