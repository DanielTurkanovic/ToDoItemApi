using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Data
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>()
                .Property(t => t.IsCompleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<ToDoItem>()
               .HasQueryFilter(t => !t.IsDeleted);

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
