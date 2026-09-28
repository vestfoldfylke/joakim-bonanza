using Backend.Services;

namespace Backend.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this WebApplication app)
    {
        app.MapGet("/items", async (IItemRepository repository) =>
        {
            return await repository.GetAllItemsAsync();
        })
        .WithName("GetItems");
    }
}