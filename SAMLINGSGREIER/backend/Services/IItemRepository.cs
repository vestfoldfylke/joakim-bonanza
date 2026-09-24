using Backend.Models;

namespace Backend.Services;

public interface IItemRepository
{
    IEnumerable<Item> GetAllItems();
}