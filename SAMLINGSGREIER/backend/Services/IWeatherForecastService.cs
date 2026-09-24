using Backend.Models;
namespace Backend.Services;

public interface IWeatherForecastService
{
    WeatherForecast[] GetForecast(int days);
}
