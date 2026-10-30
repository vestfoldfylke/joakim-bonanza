using Backend.Services;

namespace Backend.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", async (ICategoryRepository repository) =>
        {
            return await repository.GetAllCategoriesAsync();
        })
        .WithName("GetCategories")
        .RequireAuthorization(p => p.RequireRole("Administrator"));
    }
}
