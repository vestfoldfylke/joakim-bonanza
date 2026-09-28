using Backend.Services;
using Backend.Models;

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

        app.MapPost("items", async (AddItemRequest request, IItemRepository repository) =>
        {
            var item = await repository.AddItemAsync(request);
            return Results.Created($"/items/{item.Id}", item);
        })
        .WithName("AddItem");
    }
}