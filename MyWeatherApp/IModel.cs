using System.Threading.Tasks;

namespace MyWeatherApp
{
    public interface IModel
    {
        Task<WeatherNow> GetWeatherNow();
    }
}