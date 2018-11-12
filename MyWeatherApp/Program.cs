using System;
using System.Linq;
using MyWeatherApp.LocationsRepository;

namespace MyWeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller(args, new LocationsContext());
            //controller.ForTestOnly();
        }
    }
}