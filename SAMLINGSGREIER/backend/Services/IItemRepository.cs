using Backend.Models;

namespace Backend.Services;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<Item> AddItemAsync(AddItemRequest request);
    Task<bool> RemoveItemAsync(Guid id);
    Task<Item?> GetItemByIdAsync(Guid id);

}