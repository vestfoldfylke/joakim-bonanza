using Backend.Services;
using Backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

if (builder.Environment.IsDevelopment() && allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "Cors:AllowedOrigins er tom. Opprett appsettings.Development.json med frontend-URL, se README.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.MapItemEndpoints();

app.Run();
