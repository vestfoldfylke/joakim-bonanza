using Backend.Data;
using Backend.Models;

namespace Backend.Services;

public class SqliteItemRepository : IItemRepository
{
    private readonly CollectionDbContext _db;

    public SqliteItemRepository(CollectionDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Item> GetAllItems() => _db.Items.ToList();
}