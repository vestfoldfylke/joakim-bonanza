namespace Backend.Models;

public record Item(Guid Id, string Name, Guid CategoryId, DateTime AddedAt);
