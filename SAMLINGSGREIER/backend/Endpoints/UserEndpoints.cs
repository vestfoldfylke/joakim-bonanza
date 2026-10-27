using Backend.Models;
using Backend.Services;

namespace Backend.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users")
            .RequireAuthorization(p => p.RequireRole("Administrator"));

        group.MapGet("/", async (IUserRepository users) =>
            (await users.GetAllUsersAsync()).Select(UserResponse.From))
        .WithName("GetUsers");

        group.MapPost("/", async (AddUserRequest request, IUserRepository users, IGroupRepository groups) =>
        {
            if (string.IsNullOrWhiteSpace(request.Username) || request.Password.Length < 12)
                return Results.BadRequest("Brukernavn må fylles ut, og passordet må ha minst 12 tegn.");

            if (await groups.GetGroupByIdAsync(request.GroupId) is null)
                return Results.BadRequest("Gruppen finnes ikke.");

            try
            {
                var user = await users.AddUserAsync(request);
                return Results.Created($"/users/{user.Id}", UserResponse.From(user));
            }
            catch (InvalidOperationException e)
            {
                return Results.Conflict(e.Message);
            }
        })
        .WithName("AddUser");

        group.MapDelete("/{id:guid}", async (Guid id, IUserRepository users) =>
            await users.RemoveUserAsync(id) ? Results.NoContent() : Results.NotFound())
        .WithName("RemoveUser");
    }
}
