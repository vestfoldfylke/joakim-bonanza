using Backend.Services;
using Backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapItemEndpoints();

app.Run();
