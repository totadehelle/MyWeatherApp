using System;
using System.Linq;

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
            
            //For testing only!
            /*string locationId;
            using(builder._context)
            {
                var cityId = from city in builder._context.Cities
                    where city.Name == "Moscow"
                    select city;

                if (cityId.Count() > 1)
                {
                    foreach (var city in cityId)
                    {
                        Console.WriteLine($"City: {city.Name} \n Country: {city.Country} \n Coordinates:");
                        foreach (var pair in city.Coord)
                        {
                            Console.WriteLine(pair.Key + ": " + pair.Value);
                        }
                    }

                    Console.WriteLine("Please select your city and print its id:");
                    locationId = Console.ReadLine();
                }

                locationId = cityId.First().Id.ToString();
            }*/
            
            builder.GetLocationIdDb();
            //Controller controller = new Controller(args);
        }
    }
}