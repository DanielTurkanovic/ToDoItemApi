using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Data
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

        public DbSet<ToDoItems> ToDoItems { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItems>()
                .Property(t => t.IsCompleted)
                .HasDefaultValue(false);
        }
    }
}
