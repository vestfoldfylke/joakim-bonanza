namespace Backend.Models;

public record AddUserRequest(string Username, string FirstName, string LastName, Guid GroupId, string Password);
