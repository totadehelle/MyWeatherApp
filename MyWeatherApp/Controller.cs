using System;
using System.Threading;
using MyWeatherApp.LocationsRepository;
using System.Linq;
using MyWeatherApp.CommandLine.Utility;

namespace MyWeatherApp
{
    public class Controller
    {
        private IModel _model;
        private View _view;
        private IRepository _repository;
        private string _location;
        private int _daysAhead = 0;
        private Arguments _cmdline;
        
        
        public Controller(string[] args, IRepository repository)
        {
            _cmdline = new Arguments(args);
            _view = new View();
            
            using (LocationsContext context = new LocationsContext())
            {
                context.Database.EnsureCreated();
                FillDbIfEmpty(context);
            }
            
            _repository = repository;

            Process();
        }

        private void Process()
        {
            _location = _cmdline["location"] ?? GetCurrentLocation();
            
            string locationId = GetLocationId(_location);

            if (locationId == null) return;

            if (_cmdline["d"] != null)
            {
                bool isInt = Int32.TryParse(_cmdline["d"], out _daysAhead);
            }

            if (_daysAhead < 0)
            {
                Console.WriteLine("-d value cannot be less than 0");
                return;
            }
            
            _model = new TimeLimitProxy(locationId, _daysAhead);
            var weather = _model.GetWeather();
            _view.ShowWeather(weather);
        }

        private void FillDbIfEmpty(LocationsContext context)
        {
            if (context.Cities.Count() == 0)
            {
                CitiesDbBuilder builder = new CitiesDbBuilder();
                builder.MakeCitiesDbFromJson();
            }
        }

        private string GetCurrentLocation()
        {
            //определение города по IP
            return null;
        }

        private string GetLocationId(string location)
        {
            string locationId = null;
            var citiesFound = _repository.GetCityList(location);
            if (citiesFound.Count() == 0)
            {
                Console.WriteLine("The city is not found.");
            }
                
            if (citiesFound.Count() > 1)
            {
                foreach (var city in citiesFound)
                {
                    Console.WriteLine($"ID: {city.Id} \n City: {city.Name} \n Country: {city.Country} \n Coordinates: \n longitude: {city.Lon}, \n latitude: {city.Lat} \n");
                }

                Console.WriteLine("Please select your city and print its ID:");
                string userInput = "default";
                while (!citiesFound.Any(c => c.Id.ToString() == userInput))
                {
                    userInput = Console.ReadLine();
                    
                    if (citiesFound.Any(c => c.Id.ToString() == userInput))
                    {
                        locationId = userInput;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect city ID, please try again");
                    }
                }
            }
                
            else locationId = citiesFound.First().Id.ToString();

            return locationId;
        }
    }
}