namespace Backend.Models;

public record User(Guid Id, string Username, string FirstName, string LastName, Guid GroupId, string PasswordHash);
