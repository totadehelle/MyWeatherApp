using MyWeatherApp.WeatherModels;

namespace MyWeatherApp
{
    public interface IModel
    {
        IWeather GetWeather();
    }
}