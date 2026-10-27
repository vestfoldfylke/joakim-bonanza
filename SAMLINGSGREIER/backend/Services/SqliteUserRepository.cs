using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services;

public class SqliteUserRepository : IUserRepository
{
    private readonly CollectionDbContext _db;
    private readonly IPasswordHasher<User> _hasher;

    public SqliteUserRepository(CollectionDbContext db, IPasswordHasher<User> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() => await _db.Users.ToListAsync();

    public async Task<User> AddUserAsync(AddUserRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Username == request.Username))
            throw new InvalidOperationException("Brukernavnet er allerede i bruk.");

        var user = new User(
            Guid.NewGuid(),
            request.Username,
            request.FirstName,
            request.LastName,
            request.GroupId,
            ""
        );

        user = user with { PasswordHash = _hasher.HashPassword(user, request.Password) };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> VerifyPasswordAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Failed ? null : user;
    }

    public async Task<bool> RemoveUserAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByIdAsync(Guid id) =>
        await _db.Users.FindAsync(id);
}
