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

        public async Task<ToDoItems> CreateAsync(ToDoItems toDoItems)
        {
            toDoItems.CreatedAt = DateTime.UtcNow;

            await dbContext.ToDoItems.AddAsync(toDoItems);
            await dbContext.SaveChangesAsync();

            return toDoItems;
        }

        public async Task<List<ToDoItems>> GetAllAsync()
        {
            var result = await dbContext.ToDoItems.ToListAsync();
            return result;
        }

        public async Task<ToDoItems?> GetByIdAsync(int id)
        {
            return await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ToDoItems>> SearchByTitleAndDescriptionAsync(string title, string description)
        {
            var query = dbContext.ToDoItems.AsQueryable();

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

        public async Task<ToDoItems> UpdateAsync(ToDoItems toDoItem)
        {
            // Find the existing item in the database
            var existingItem = await dbContext.ToDoItems
                .FirstOrDefaultAsync(x => x.Id == toDoItem.Id);

            if (existingItem == null)
            {
                return null;
            }

            // Check and update Title/Description
            existingItem.Title = toDoItem.Title;
            existingItem.Description = toDoItem.Description;
            existingItem.IsCompleted = toDoItem.IsCompleted;
            var date = DateTime.UtcNow;

            // If there has been a change in IsCompleted, set CompletedAt.
            if (toDoItem.IsCompleted == true)
            {
                    existingItem.CompletedAt =  date;
            }
            else
            {
                existingItem.CompletedAt = null;
            }

                // Set UpdatedAt
                existingItem.UpdatedAt = date;

            // Update entity
            dbContext.Update(existingItem);

            // Save changes
            await dbContext.SaveChangesAsync();

            return existingItem;
        }


        public async Task<ToDoItems?> DeleteAsync(int id)
        {
            var existingItem = await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == id);

            dbContext.ToDoItems.Remove(existingItem);
            await dbContext.SaveChangesAsync();

            return existingItem;
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await dbContext.ToDoItems
                .AnyAsync(x => x.Title.ToLower() == title.ToLower());
        }
    }
}

