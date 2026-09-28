using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

public class CollectionDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    public CollectionDbContext(DbContextOptions<CollectionDbContext> options) : base(options) {  }
}