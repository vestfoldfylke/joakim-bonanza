using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SqliteItemRepository : IItemRepository
{
    private readonly CollectionDbContext _db;

    public SqliteItemRepository(CollectionDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync() => await _db.Items.ToListAsync();
}