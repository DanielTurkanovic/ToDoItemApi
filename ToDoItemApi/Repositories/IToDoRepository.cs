using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public interface IToDoRepository
    {
        Task<ToDoItems> CreateAsync(ToDoItems toDoItems, int userId);
        Task<List<ToDoItems>> GetAllAsync(int userId);
        Task<ToDoItems?> GetByIdAsync(int id, int userId);

        // This could be IReadOnlyList<ToDoItems?> in case we want strict reading
        Task<List<ToDoItems?>> SearchByTitleAndDescriptionAsync(string title, string description, int userId);
        Task <ToDoItems> UpdateAsync(ToDoItems toDoItem, int userId);
        Task<ToDoItems> DeleteAsync(int id, int userId);
        Task<bool> ExistsByTitleAsync(string title, int userId);

    }
}
