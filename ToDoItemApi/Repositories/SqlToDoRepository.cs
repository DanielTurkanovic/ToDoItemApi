using Microsoft.EntityFrameworkCore;  
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Repositories
{
    public class SqlToDoRepository : IToDoRepository
    {
        private readonly ToDoDbContext dbContext;

        public SqlToDoRepository(ToDoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ToDoItems> CreateAsync(ToDoItems toDoItems, string userId)
        {
            toDoItems.CreatedAt = DateTime.UtcNow;
            toDoItems.UserId = userId;

            await dbContext.ToDoItems.AddAsync(toDoItems);
            await dbContext.SaveChangesAsync();

            return toDoItems;
        }

        public async Task<List<ToDoItems>> GetAllAsync(string userId)
        {
            return await dbContext.ToDoItems
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<ToDoItems?> GetByIdAsync(int id, string userId)
        {
            return await dbContext.ToDoItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<List<ToDoItems>> SearchByTitleAndDescriptionAsync(string title, string description, string userId)
        {
            var query = dbContext.ToDoItems.Where(x => x.UserId == userId);

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

        public async Task<ToDoItems> UpdateAsync(ToDoItems toDoItem, string userId)
        {
            // Find the existing item in the database
            var existingItem = await dbContext.ToDoItems
                .FirstOrDefaultAsync(x => x.Id == toDoItem.Id && x.UserId == userId);

            if (existingItem == null)
            {
                return null;
            }

            // Check and update Title/Description
            existingItem.Title = toDoItem.Title;
            existingItem.Description = toDoItem.Description;
            existingItem.IsCompleted = toDoItem.IsCompleted;

            var date = DateTime.UtcNow;

            existingItem.UpdatedAt = date;
            existingItem.CompletedAt = toDoItem.IsCompleted == true ? date : null;

            // Update entity
            dbContext.Update(existingItem);

            // Save changes
            await dbContext.SaveChangesAsync();

            return existingItem;
        }


        public async Task<ToDoItems?> DeleteAsync(int id, string userId)
        {
            var existingItem = await dbContext.ToDoItems.
                FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (existingItem == null) 
            {
                return null;
            }

            dbContext.ToDoItems.Remove(existingItem);
            await dbContext.SaveChangesAsync();

            return existingItem;
        }

        public async Task<bool> ExistsByTitleAsync(string title, string userId)
        {
            return await dbContext.ToDoItems
                .AnyAsync(x => x.Title.ToLower() == title.ToLower() && x.UserId == userId);
        }
    }
}

