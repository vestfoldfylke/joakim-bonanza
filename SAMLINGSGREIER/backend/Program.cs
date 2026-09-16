var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("dev");

app.MapGet("/items", () => new[]
{
    new { Id = 1, Name = "Rubik's cube" },
    new { Id = 2, Name = "Vinyl: Kind of Blue" }
});

app.Run();
