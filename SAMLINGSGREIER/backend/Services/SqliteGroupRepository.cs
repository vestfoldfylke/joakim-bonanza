using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SqliteGroupRepository : IGroupRepository
{
    private readonly CollectionDbContext _db;

    public SqliteGroupRepository(CollectionDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Group>> GetAllGroupsAsync() => await _db.Groups.ToListAsync();
    public async Task<Group?> GetGroupByIdAsync(Guid id) => await _db.Groups.FindAsync(id);
}
