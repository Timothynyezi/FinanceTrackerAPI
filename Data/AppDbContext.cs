using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Models;

namespace FinanceTrackerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure usernames are unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Seed default categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Salary" },
            new Category { Id = 2, Name = "Food & Groceries" },
            new Category { Id = 3, Name = "Transport" },
            new Category { Id = 4, Name = "Utilities" },
            new Category { Id = 5, Name = "Entertainment" },
            new Category { Id = 6, Name = "Healthcare" },
            new Category { Id = 7, Name = "Other" }
        );

        // Decimal precision for Amount
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)");
    }
}
