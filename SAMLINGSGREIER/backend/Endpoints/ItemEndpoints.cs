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

        app.MapGet("/items/{id:guid}", async (Guid id, IItemRepository repository) =>
        {
            var item = await repository.GetItemByIdAsync(id);
            return item is null ? Results.NotFound() : Results.Ok(item);
        })
        .WithName("GetItemById");


        app.MapPost("/items", async (AddItemRequest request, IItemRepository repository) =>
        {
            var errors = new Dictionary<string, string[]>();

            if(string.IsNullOrWhiteSpace(request.Name))
                errors["name"] = ["Name cannot be empty"];
            
            if(string.IsNullOrWhiteSpace(request.Category))
                errors["category"] = ["Category cannot be empty"];
            
            if (errors.Count > 0)
                return Results.ValidationProblem(errors);

            var item = await repository.AddItemAsync(request);
            return Results.Created($"/items/{item.Id}", item);
        })
        .WithName("AddItem");
    }
}