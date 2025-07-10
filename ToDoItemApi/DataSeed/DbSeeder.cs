using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.DataSeed
{
    public static class DbSeeder
    {
        public static void SeedUsers(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            // Apply migrations if needed
            db.Database.Migrate();

            // If there are no users, add two
            if (!db.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        IsAdmin = true,
                        IsDeleted = false
                    },
                    new User
                    {
                        Email = "user@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        IsAdmin = false,
                        IsDeleted = false
                    }
                };

                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }
    }
}
