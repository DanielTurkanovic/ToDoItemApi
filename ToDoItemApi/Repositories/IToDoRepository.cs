using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public interface IToDoRepository
    {
        Task<ToDoItems> CreateAsync(ToDoItems toDoItems, string userId);
        Task<List<ToDoItems>> GetAllAsync(string userId);
        Task<ToDoItems?> GetByIdAsync(int id, string userId);

        // This could be IReadOnlyList<ToDoItems?> in case we want strict reading
        Task<List<ToDoItems?>> SearchByTitleAndDescriptionAsync(string title, string description, string userId);
        Task <ToDoItems> UpdateAsync(ToDoItems toDoItem, string userId);
        Task<ToDoItems> DeleteAsync(int id, string userId);
        Task<bool> ExistsByTitleAsync(string title, string userId);

    }
}
