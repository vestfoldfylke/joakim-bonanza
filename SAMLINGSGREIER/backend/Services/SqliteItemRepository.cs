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

    public async Task<Item> AddItemAsync(AddItemRequest request)
    {
        var item = new Item(
            Guid.NewGuid(),
            request.Name,
            request.CategoryId,
            DateTime.UtcNow
        );

        _db.Items.Add(item);
        await _db.SaveChangesAsync();

        return item;
    }

    public async Task<bool> RemoveItemAsync(Guid id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item is null) return false;

        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Item?> GetItemByIdAsync(Guid id) =>
        await _db.Items.FindAsync(id);
}
