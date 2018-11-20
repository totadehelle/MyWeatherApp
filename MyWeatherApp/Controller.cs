using System;
using MyWeatherApp.Repositories;
using System.Linq;
using MyWeatherApp.Utility;
using MyWeatherApp.WeatherModels;
using AppContext = MyWeatherApp.Repositories.AppContext;

namespace MyWeatherApp
{
    public class Controller
    {
        private IModel _model;
        private readonly View _view;
        private readonly ICitiesRepository _citiesRepository;
        private readonly ICashedForecastsRepository _cashRepository;
        private string _location;
        private int _daysAhead = 0;
        private readonly Arguments _cmdline;
        
        
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
            #region CMD_ARGS_PROCESSING
            
            if (_cmdline["help"] != null)
            {
                Console.WriteLine("This app can show current Weather or the Weather forecast for chosen city. \n" +
                                  "There are the following parameters available: \n" +
                                  "--help - shows this manual. \n" +
                                  "--location - sets the city. Example: --location London. Without this parameter the app will use default city if set. \n" +
                                  "-d - sets number of days ahead. Example: -d 1. -d value can be from 0 (today) \n" +
                                  " to 5 (five days ahead). By default -d value is 0. Negative numbers will be replaced by 0.\n" +
                                  "-f - sets chosen city as default. Example: --location London -f. \n" +
                                  "All the parameters are not mandatory.");
                return;
            }

            string locationId;
            
            if (_cmdline["location"] != null)
            {
                _location = _cmdline["location"];
                locationId = GetLocationId(_location);
            }
            else
            {
                locationId = GetCurrentLocation();
            }

            if (locationId == null)
            {
                Console.WriteLine("Please choose your city using --location command; you can also set any city as default using command -f.");
                return;
            }

            if (_cmdline["f"] != null)
            {
                _citiesRepository.SetFavourite(Int32.Parse(locationId));
            }

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
            #endregion

            // It's reasonable to get forecast for other days once a day, and use one cashed response within one day.
            if (type == WeatherType.Forecast) 
            {
                if(CheckCash(int.Parse(locationId), type)) return;
            }
            
            _model = new TimeLimitProxy(locationId, _daysAhead);
            
            var weather = _model.GetWeather();

            if (weather == null)
            {
                // Current Weather may change during the day, so we need cash only if we cannot get actual info.
                CheckCash(int.Parse(locationId), type); 
                return;
            }
            
            if (type == WeatherType.Forecast)
            {
                var weatherForecast = weather as WeatherForecast;
                var dayRequired = (from day in weatherForecast.List where day.Date.Date == (DateTime.Today.AddDays(_daysAhead)) select day).ToList();
                weatherForecast.List = dayRequired;
                weather = weatherForecast;
            }
            
            var message = _view.ShowWeather(weather, type);
            
            _cashRepository.Add(_cashRepository.Create(weather, message, type, _daysAhead));
        }

        private void FillDbIfEmpty(AppContext context)
        {
            if (context.Cities.Any()) return;
            var builder = new AppDbBuilder();
            builder.MakeCitiesDbFromJson();
        }

        private string GetCurrentLocation()
        {
            return _citiesRepository.GetFavourite()?.Id.ToString();
        }

        private string GetLocationId(string location)
        {
            string locationId = null;
            var citiesFound = _citiesRepository.Get(location);
            if (!citiesFound.Any())
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
                var userInput = "default";
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

        private bool CheckCash(int locationId, WeatherType type)
        {
            try
            {
                var forecastsFound = _cashRepository.Get(locationId, _daysAhead, type);
                if (forecastsFound.Any())
                {
                    Console.WriteLine("Cashed data: \n");
                    Console.WriteLine(forecastsFound.Last().Message);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                // if the table was not created, there is no cash
                return false;
            }
        }
        
        
        

    }
}