using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, bool includeDeleted = false);
        Task<bool> RestoreUserAsync(int id);
        Task<List<User>> GetDeletedUsersAsync();
        Task<bool> SoftDeleteUserAsync(int id);
    }
}
