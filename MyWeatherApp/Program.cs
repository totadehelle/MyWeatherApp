using MyWeatherApp.Repositories;

namespace MyWeatherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller(args, new SqliteCitiesRepository(), new SqliteCashedForecastsRepository());
        }
    }
}