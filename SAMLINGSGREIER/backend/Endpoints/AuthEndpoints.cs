using System.Security.Claims;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Backend.Endpoints;

// Takk claude
public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/login", async (LoginRequest request, IUserRepository users,
            IGroupRepository groups, HttpContext http) =>
        {
            var user = await users.VerifyPasswordAsync(request.Username, request.Password);
            if (user is null) return Results.Unauthorized();

            var group = await groups.GetGroupByIdAsync(user.GroupId);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, group?.Name ?? "")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return Results.Ok(new { user.Username, Role = group?.Name });
        })
        .WithName("Login");

        app.MapPost("/auth/logout", async (HttpContext http) =>
        {
            await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.NoContent();
        })
        .WithName("Logout");

        app.MapGet("/auth/me", (ClaimsPrincipal user) => Results.Ok(new
        {
            Username = user.Identity!.Name,
            Role = user.FindFirstValue(ClaimTypes.Role)
        }))
        .RequireAuthorization()
        .WithName("Me");
    }
}
