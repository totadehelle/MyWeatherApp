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
        public readonly LocationsContext _context;
        
        public Controller(string[] args, LocationsContext context)
        {
            _model = new Model();
            _view = new View();
            _context = context;
            _context.Database.EnsureCreated();
            FillDbIfEmpty();
        }

        private void FillDbIfEmpty()
        {
            if (_context.Cities.Count() == 0)
            {
                CitiesDbBuilder builder = new CitiesDbBuilder();
                builder.MakeCitiesDbFromJson();
            }
        }

        public void ForTestOnly()
        {
            string locationId;
            
            var citiesFound = from city in _context.Cities
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
                Console.WriteLine(locationId);
                return;
            }

            locationId = citiesFound.First().Id.ToString();
            Console.WriteLine(locationId);
        }
    }
}