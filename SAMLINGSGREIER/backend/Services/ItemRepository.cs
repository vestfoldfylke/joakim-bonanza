using Backend.Models;

namespace Backend.Services;

public class ItemRepository : IItemRepository
{
    private static readonly List<Item> _items = new List<Item>
    {
        new(Guid.NewGuid(), "Gameboy", "Konsoller", DateTimeOffset.Now),
        new(Guid.NewGuid(), "Morgan Elgitar", "Musikk", DateTimeOffset.Now),
    };

    public IEnumerable<Item> GetAllItems() => _items;
}