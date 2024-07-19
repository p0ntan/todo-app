using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("User")
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Todo>().ToTable("Todo");
        modelBuilder.Entity<RefreshToken>()
            .ToTable("RefreshToken")
            .HasIndex(u => u.UserId)
            .IsUnique();
    }
}
