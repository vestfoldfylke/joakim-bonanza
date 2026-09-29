using Backend.Models;

namespace Backend.Services;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<Item> AddItemAsync(AddItemRequest request);
    Task<Item?> GetItemByIdAsync(Guid id);

}