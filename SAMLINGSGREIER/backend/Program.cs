using Backend.Services;
using Backend.Endpoints;
using Backend.Data;
using Backend.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<IItemRepository, SqliteItemRepository>();
builder.Services.AddScoped<ICategoryRepository, SqliteCategoryRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserRepository, SqliteUserRepository>();
builder.Services.AddScoped<IGroupRepository, SqliteGroupRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "samling.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        options.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
        options.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };

        options.Events.OnValidatePrincipal = async ctx =>
        {
            var idValue = ctx.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var services = ctx.HttpContext.RequestServices;

            var user = Guid.TryParse(idValue, out var userId)
                ? await services.GetRequiredService<IUserRepository>().GetUserByIdAsync(userId)
                : null;

            if (user is null)
            {
                ctx.RejectPrincipal();
                await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return;
            }

            var group = await services.GetRequiredService<IGroupRepository>().GetGroupByIdAsync(user.GroupId);
            var currentRole = group?.Name ?? "";

            if (ctx.Principal!.FindFirstValue(ClaimTypes.Role) != currentRole)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, currentRole)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ctx.ReplacePrincipal(new ClaimsPrincipal(identity));
                ctx.ShouldRenew = true;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddValidation();

var connectionString = builder.Configuration.GetConnectionString("Samling")
    ?? throw new InvalidOperationException(
        "Missing 'ConnectionStrings:Samling' in appsettings.json. Look at README.md.");

builder.Services.AddDbContext<CollectionDbContext>(options =>
    options.UseSqlite(connectionString));

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var adminPassword = app.Configuration["SeedAdmin:Password"];

    if (!string.IsNullOrEmpty(adminPassword) && !(await users.GetAllUsersAsync()).Any())
    {
        var adminGroupId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        await users.AddUserAsync(new AddUserRequest("admin", "Admin", "Bruker", adminGroupId, adminPassword));
        app.Logger.LogInformation("Opprettet første admin-bruker 'admin'.");
    }
}


if (allowedOrigins.Length == 0)
{
    app.Logger.LogWarning(
        "Cors:AllowedOrigins er tom. Alle cross-origin-kall vil bli avvist. Se README.");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapItemEndpoints();
app.MapCategoryEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();
