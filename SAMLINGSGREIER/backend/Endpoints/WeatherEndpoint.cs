using Backend.Services;
namespace Backend.Endpoints;

public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast", (IWeatherForecastService service) =>
        {
            return service.GetForecast(5);
        })
        .WithName("GetWeatherForecast");
    }
}
