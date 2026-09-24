namespace Backend.Services;
using Backend.Models;

public class ItemRepository : IItemRepository
{
    private readonly List<Item> _items;

    public ItemRepository()
    {
        _items = new List<Item>
        {
            new(Guid.NewGuid(), "Gameboy", "Konsoller", DateTime.Now),
            new(Guid.NewGuid(), "Morgan Elgitar", "Musikk", DateTime.Now),
        };
    }
    public IEnumerable<Item> GetAllItems() => _items;
}