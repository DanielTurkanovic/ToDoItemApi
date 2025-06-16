using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public interface IToDoRepository
    {
        Task<ToDoItems> CreateAsync(ToDoItems toDoItemApi);
        Task<List<ToDoItems>> GetAllAsync();
        Task<ToDoItems?> GetByIdAsync(int id);

        // This could be IReadOnlyList<ToDoItems?> in case we want strict reading
        Task<List<ToDoItems?>> SearchByTitleAndDescriptionAsync(string title, string description);
        Task <ToDoItems> UpdateAsync(ToDoItems toDoItem);
        Task<ToDoItems> DeleteAsync(int id);
        Task<bool> ExistsByTitleAsync(string title);

    }
}
