using MyWeatherApp.Repositories;

namespace MyWeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new Controller(args, new SqliteCitiesRepository(), new SqliteCashedForecastsRepository());
        }
    }
}