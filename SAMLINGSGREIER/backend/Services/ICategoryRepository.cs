using Backend.Models;

namespace Backend.Services;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();

}
