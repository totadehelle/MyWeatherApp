using System;
using MyWeatherApp.LocationsRepository;
using System.Linq;
using MyWeatherApp.CommandLine.Utility;
using MyWeatherApp.WeatherModels;
using AppContext = MyWeatherApp.LocationsRepository.AppContext;

namespace MyWeatherApp
{
    public class Controller
    {
        private IModel _model;
        private View _view;
        private ICitiesRepository _citiesRepository;
        private ICashedForecastsRepository _cashRepository;
        private string _location;
        private int _daysAhead = 0;
        private Arguments _cmdline;
        
        
        public Controller(string[] args, ICitiesRepository citiesRepository, ICashedForecastsRepository cashRepository)
        {
            _cmdline = new Arguments(args);
            _view = new View();
            
            using (AppContext context = new AppContext())
            {
                context.Database.EnsureCreated();
                FillDbIfEmpty(context);
            }
            
            _citiesRepository = citiesRepository;
            _cashRepository = cashRepository;

            Process();
        }

        private void Process()
        {
            _location = _cmdline["location"] ?? GetCurrentLocation();
            
            string locationId = GetLocationId(_location);

            if (locationId == null) return;

            WeatherType type = WeatherType.Current;

            if (_cmdline["d"] != null)
            {
                bool isInt = Int32.TryParse(_cmdline["d"], out _daysAhead);
                if (_daysAhead < 0)
                {
                    Console.WriteLine("-d value cannot be less than 0");
                    return;
                }

                if (_daysAhead > 5)
                {
                    Console.WriteLine("-d value can be from 0 to 5");
                    return;
                }
                if (_daysAhead > 0) type = WeatherType.Forecast;
            }

            if(CheckCash(Int32.Parse(locationId), type)) return;
            
            _model = new TimeLimitProxy(locationId, _daysAhead);
            var weather = _model.GetWeather();
            string message = null;
            
            if (type == WeatherType.Current)
            {
                CurrentWeather currentWeather = weather as CurrentWeather;
                message = _view.ShowCurrentWeather(currentWeather);
            }

            if (type == WeatherType.Forecast)
            {
                WeatherForecast weatherForecast = weather as WeatherForecast;
                var dayRequired = (from day in weatherForecast.list where day.Date.Date == (DateTime.Today.AddDays(_daysAhead)) select day).ToList();
                weatherForecast.list = dayRequired;
                message = _view.ShowWeatherForecast(weatherForecast);
            }
            
            _cashRepository.Add(_cashRepository.Create(weather, message, type, _daysAhead));
        }

        private void FillDbIfEmpty(AppContext context)
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
            var citiesFound = _citiesRepository.Get(location);
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

        public bool CheckCash(int locationID, WeatherType type)
        {
            var forecastsFound = _cashRepository.Get(locationID, _daysAhead, type);
            if (forecastsFound.Count() > 0)
            {
                Console.WriteLine("Cashed data: \n");
                Console.WriteLine(forecastsFound.First().Message);
                return true;
            }

            return false;
        }
    }
}