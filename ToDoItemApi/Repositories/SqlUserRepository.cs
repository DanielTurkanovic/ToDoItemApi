using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly ToDoDbContext dbContext;

        public SqlUserRepository(ToDoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var query = includeDeleted
                ? dbContext.Users.IgnoreQueryFilters()
                : dbContext.Users;

            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> RestoreUserAsync(int id)
        {
            var user = await GetByIdAsync(id, includeDeleted: true);

            if (user == null || !user.IsDeleted)
                return false;

            user.IsDeleted = false;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetDeletedUsersAsync()
        {
            return await dbContext.Users
                .IgnoreQueryFilters()
                .Where(u => u.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await GetByIdAsync(id, includeDeleted: true);

            if (user == null || user.IsDeleted)
                return false;

            user.IsDeleted = true;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
