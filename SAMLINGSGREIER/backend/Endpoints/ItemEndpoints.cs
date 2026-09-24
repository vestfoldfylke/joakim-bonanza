using Backend.Services;
namespace Backend.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this WebApplication app)
    {
        app.MapGet("/items", (IItemRepository service) =>
        {
            return service.GetAllItems();
        })
        .WithName("GetItems");
    }
}