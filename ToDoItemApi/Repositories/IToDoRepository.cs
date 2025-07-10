using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public interface IToDoRepository
    {
        Task<ToDoItem> CreateAsync(ToDoItem toDoItems, int userId);
        Task<List<ToDoItem>> GetAllAsync(int userId);
        Task<ToDoItem?> GetByIdAsync(int id, int userId);

        // This could be IReadOnlyList<ToDoItems?> in case we want strict reading
        Task<List<ToDoItem?>> SearchByTitleAndDescriptionAsync(string title, string description, int userId);
        Task <ToDoItem> UpdateAsync(ToDoItem toDoItem, int userId);
        Task<ToDoItem> DeleteAsync(int id, int userId);
        Task<bool> ExistsByTitleAsync(string title, int userId);

    }
}
