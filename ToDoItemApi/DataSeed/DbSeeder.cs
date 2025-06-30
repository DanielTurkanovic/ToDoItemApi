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

            // Primeni migracije ako treba
            db.Database.Migrate();

            // Ako nema nijednog korisnika, dodaj dva
            if (!db.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Email = "admin@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
                    },
                    new User
                    {
                        Email = "user@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!")
                    }
                };

                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }
    }
}
