using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<LifetimeService1>();
builder.Services.AddScoped<LifetimeService2>();
builder.Services.AddSingleton<LifetimeService3>();

builder.Services.AddScoped<ManInTheMiddle>();

var app = builder.Build();

app.MapGet("/", (LifetimeService1 transient, LifetimeService2 scoped, LifetimeService3 singleton, ManInTheMiddle manInTheMiddle) =>
{   
    var transientData = new
    {
        endpointId = transient.Id,
        middlemanId = manInTheMiddle.GetTransientGuid(),
        count = transient.IncrementCount()
    };

    var scopedData = new
    {
        endpointId = scoped.Id,
        middlemanId = manInTheMiddle.GetScopedGuid(),
        count = scoped.IncrementCount()
    };

    var singletonData = new
    {
        endpointId = singleton.Id,
        middlemanId = manInTheMiddle.GetSingletonGuid(),
        count = singleton.IncrementCount()
    };

    return new
    {
        transientData, scopedData, singletonData
    };
});

app.Run();

public class LifetimeService1
{
    public Guid Id { get; set; } = Guid.NewGuid();
    int Count { get; set; } = 0;

    public int IncrementCount()
    {
        Count++;
        return Count;
    }
}

public class LifetimeService2
{
    public Guid Id { get; set; } = Guid.NewGuid();
    int Count { get; set; } = 0;

    public int IncrementCount()
    {
        Count++;
        return Count;
    }
}

public class LifetimeService3
{
    public Guid Id { get; set; } = Guid.NewGuid();
    int Count { get; set; } = 0;

    public int IncrementCount()
    {
        Count++;
        return Count;
    }
}

public class ManInTheMiddle
{
    private LifetimeService1 _transientService;
    private LifetimeService2 _scopedService;
    private LifetimeService3 _singletonService;

    public ManInTheMiddle(LifetimeService1 transient, LifetimeService2 scoped, LifetimeService3 singleton)
    {
        _transientService = transient;
        _scopedService = scoped;
        _singletonService = singleton;
    }

    public Guid GetTransientGuid()
    {
        return _transientService.Id;
    }

    public Guid GetScopedGuid()
    {
        return _scopedService.Id;
    }

    public Guid GetSingletonGuid()
    {
        return _singletonService.Id;
    }
}
