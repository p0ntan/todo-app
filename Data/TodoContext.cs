using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("User")
            .HasIndex(e => e.Email)
            .HasDatabaseName("IX_User_Email");
        modelBuilder.Entity<Todo>().ToTable("Todo");
    }
}
