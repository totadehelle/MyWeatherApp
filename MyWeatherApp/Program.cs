using System;
using System.Linq;
using MyWeatherApp.LocationsRepository;

namespace MyWeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller(args);
            //controller.ForTestOnly();
            
            var model = new Model("2172797", 2);
            var weather = model.GetWeatherNow().Result;
            Console.WriteLine(weather.name);
            Console.WriteLine(weather.main.Temp);
        }
    }
}