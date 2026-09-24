namespace Backend.Models;

public record Item(Guid Id, string Name, string Category, DateTimeOffset DatetimeAdded);