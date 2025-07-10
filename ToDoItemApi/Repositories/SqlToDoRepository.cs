using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    // SQL-based implementation of IToDoRepository for managing ToDo items in a database.
    public class SqlToDoRepository : IToDoRepository
    {
        private readonly ToDoDbContext dbContext;

        // Constructor injecting the application's DbContext.
        public SqlToDoRepository(ToDoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Creates a new ToDo item and saves it to the database.
        public async Task<ToDoItem> CreateAsync(ToDoItem toDoItems, int userId)
        {
            toDoItems.CreatedAt = DateTime.UtcNow;
            toDoItems.UserId = userId;
            toDoItems.IsDeleted = false;

            await dbContext.ToDoItems.AddAsync(toDoItems);

            var date = DateTime.UtcNow;
            toDoItems.CompletedAt = toDoItems.IsCompleted == true ? date : null;

            await dbContext.SaveChangesAsync();

            return toDoItems;
        }

        // Retrieves all ToDo items for a specific user.
        public async Task<List<ToDoItem>> GetAllAsync(int userId)
        {
            return await dbContext.ToDoItems
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .ToListAsync();
        }

        // Retrieves a single ToDo item by ID, scoped to the current user.
        public async Task<ToDoItem?> GetByIdAsync(int id, int userId)
        {
            return await dbContext.ToDoItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId && !x.IsDeleted);
        }

        // Searches ToDo items by title and/or description, scoped to the current user.
        public async Task<List<ToDoItem>> SearchByTitleAndDescriptionAsync(string title, string description, int userId)
        {
            var query = dbContext.ToDoItems.Where(x => x.UserId == userId && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(x => x.Title != null && x.Title.ToLower().Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                query = query.Where(x => x.Description != null && x.Description.ToLower().Contains(description));
            }

            return await query.ToListAsync();
        }

        // Updates an existing ToDo item for the current user.
        public async Task<ToDoItem> UpdateAsync(ToDoItem toDoItem, int userId)
        {
            var existingItem = await dbContext.ToDoItems
                .FirstOrDefaultAsync(x => x.Id == toDoItem.Id && x.UserId == userId && !x.IsDeleted);

            if (existingItem == null)
            {
                return null;
            }

            // Update fields
            existingItem.Title = toDoItem.Title;
            existingItem.Description = toDoItem.Description;
            existingItem.IsCompleted = toDoItem.IsCompleted;

            var date = DateTime.UtcNow;
            existingItem.UpdatedAt = date;
            existingItem.CompletedAt = toDoItem.IsCompleted == true ? date : null;

            dbContext.Update(existingItem);
            await dbContext.SaveChangesAsync();

            return existingItem;
        }

        // Deletes a ToDo item by ID if it belongs to the user.
        public async Task<ToDoItem?> DeleteAsync(int id, int userId)
        {
            var existingItem = await dbContext.ToDoItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.IsDeleted = true; // Soft delete by marking as deleted
            await dbContext.SaveChangesAsync();

            return existingItem;
        }

        // Checks whether a ToDo item with the same title already exists for the user.
        public async Task<bool> ExistsByTitleAsync(string title, int userId)
        {
            return await dbContext.ToDoItems
                .AnyAsync(x => x.Title.ToLower() == title.ToLower() 
                            && x.UserId == userId
                            && !x.IsDeleted);  // Ignore soft-deleted items
        }
    }
}
