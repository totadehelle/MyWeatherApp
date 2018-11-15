using System;
using System.Threading;
using MyWeatherApp.LocationsRepository;
using System.Linq;

namespace MyWeatherApp
{
    public class Controller
    {
        private Model _model;
        private View _view;
        
        public Controller(string[] args)
        {
            //_model = new LimitProxy();
            _view = new View();
            using (LocationsContext context = new LocationsContext())
            {
                context.Database.EnsureCreated();
                FillDbIfEmpty(context);
            }
        }

        private void FillDbIfEmpty(LocationsContext context)
        {
            if (context.Cities.Count() == 0)
            {
                CitiesDbBuilder builder = new CitiesDbBuilder();
                builder.MakeCitiesDbFromJson();
            }
        }

        public void ForTestOnly()
        {
            string locationId;
            IQueryable<City> citiesFound;
            
            using (LocationsContext context = new LocationsContext())
            {
                citiesFound = from city in context.Cities
                    where city.Name == "Meget"
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
                        Console.WriteLine($"ID: {city.Id} \n City: {city.Name} \n Country: {city.Country} \n Coordinates: \n longitude: {city.Lon}, \n latitude: {city.Lat} \n");
                    }

                    Console.WriteLine("Please select your city and print its id:");
                    locationId = Console.ReadLine();
                    Console.WriteLine(locationId);
                    return;
                }
                
                locationId = citiesFound.First().Id.ToString();
            }

            Console.WriteLine(locationId);
        }
    }
}