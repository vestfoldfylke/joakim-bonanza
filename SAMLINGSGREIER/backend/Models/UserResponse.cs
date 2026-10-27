namespace Backend.Models;

public record UserResponse(Guid Id, string Username, string FirstName, string LastName, Guid GroupId)
{
    public static UserResponse From(User u) => new(u.Id, u.Username, u.FirstName, u.LastName, u.GroupId);
}
