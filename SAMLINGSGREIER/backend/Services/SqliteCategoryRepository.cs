using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SqliteCategoryRepository : ICategoryRepository
{
    private readonly CollectionDbContext _db;

    public SqliteCategoryRepository(CollectionDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync() => await _db.Categories.ToListAsync();
}
