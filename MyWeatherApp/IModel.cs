using System.Threading.Tasks;

namespace MyWeatherApp
{
    public interface IModel
    {
        IWeather GetWeather();
    }
}