using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Backend.Models;

namespace Backend.Data;

public class CollectionDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    public CollectionDbContext(DbContextOptions<CollectionDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        modelBuilder.Entity<Item>()
            .Property(e => e.AddedAt)
            .HasConversion(utcConverter);

        modelBuilder.Entity<Item>().HasData(
            new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Gameboy",
                Category = "Konsoller",
                AddedAt = new DateTime(2026, 9, 28, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Morgan Elgitar",
                Category = "Musikk",
                AddedAt = new DateTime(2026, 9, 28, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }

}