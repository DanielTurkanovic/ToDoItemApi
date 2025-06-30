using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;
using ToDoItemApi.Repositories;
using ToDoItemApi.Validators;


namespace ToDoItemNUintTests
{
    public class ToDoRepositoryTests
    {
        private ToDoDbContext dbContext;
        private SqlToDoRepository repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nasumična baza
                .Options;

            dbContext = new ToDoDbContext(options);
            repository = new SqlToDoRepository(dbContext);
        }


        [Test]
        public async Task CreateAsync_CreatesItemSuccessfully()
        {
            // Arrange
            var userId = 1;
            var newItem = new ToDoItems
            {
                Title = "New Task",
                Description = "Test Description"
            };

            // Act
            var createdItem = await repository.CreateAsync(newItem, userId);

            // Assert
            Assert.That(createdItem, Is.Not.Null);
            Assert.That(createdItem.Id, Is.GreaterThan(0));
            Assert.That(createdItem.UserId, Is.EqualTo(userId));
            Assert.That(createdItem.CreatedAt, Is.Not.EqualTo(default(DateTime)));

            var itemInDb = await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == createdItem.Id);
            Assert.That(itemInDb, Is.Not.Null);
            Assert.That(itemInDb.Title, Is.EqualTo("New Task"));
            Assert.That(itemInDb.Description, Is.EqualTo("Test Description"));
        }


        [Test]
        public async Task GetAllAsync_ReturnsOnlyItemsForGivenUser()
        {
            // Arrange
            var userId = 1;
            var otherUserId = 2;

            dbContext.ToDoItems.AddRange(
                new ToDoItems { Title = "User 1 - Clean house", Description = "Cleaning all rooms", UserId = userId },
                new ToDoItems { Title = "User 1 - Work out", Description = "Leg day", UserId = userId },
                new ToDoItems { Title = "Other User - Reeding", Description = "Reeding book", UserId = otherUserId }
            );
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(x => x.UserId == userId), Is.True);
        }


        [Test]
        public async Task GetAllAsync_ReturnsEmptyList_WhenUserHasNoItems()
        {
            // Arrange
            var userId = 99; 

            // Act
            var result = await repository.GetAllAsync(userId);

            // Assert
            Assert.That(result, Is.Empty);
        }
       

        [Test]
        public async Task GetByIdAsync_ReturnsItem_WhenExists()
        {
            // Arrange
            var userId = 1;
            var item = new ToDoItems { Title = "Chill out", Description = "Go on the beach", UserId = userId };
            dbContext.ToDoItems.Add(item);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(item.Id, userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Chill out"));
        }


        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenItemDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var missingId = 999;

            // Act
            var result = await repository.GetByIdAsync(missingId, userId);

            TestContext.WriteLine(result == null
                ? "Item not found as expected."
                : $"Unexpected item found: {result.Title}");

            // Assert
            Assert.That(result, Is.Null);
        }


        [Test]
        public async Task ExistsByTitle()
        {
            // Arrange
            var userId = 1;
            var title = "Learning";

            dbContext.ToDoItems.Add(new ToDoItems
            {
                Title = title,
                Description = "Debugging",  
                UserId = userId
            });

            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.ExistsByTitleAsync(title, userId);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task ExistsByTitleAsyncWhenItemDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var existingTitle = "Existing Task";
            var missingTitle = "Missing Task";

            dbContext.ToDoItems.Add(new ToDoItems
            {
                Title = existingTitle,
                Description = "Test description",
                UserId = userId
            });

            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.ExistsByTitleAsync(missingTitle, userId);

            // Assert
            Assert.That(result, Is.False);
        }


        [Test]
        public async Task UpdateAsync_UpdatesFieldsSuccessfully()
        {
            // Arrange
            var userId = 1;
            var original = new ToDoItems
            {
                Title = "Old Title",
                Description = "Old Desc",
                UserId = userId,
                IsCompleted = false
            };

            dbContext.ToDoItems.Add(original);
            await dbContext.SaveChangesAsync();

            var updated = new ToDoItems
            {
                Id = original.Id,
                Title = "New Title",
                Description = "New Desc",
                IsCompleted = true
            };

            // Act
            var result = await repository.UpdateAsync(updated, userId);

            // Assert
            Assert.That(result.Title, Is.EqualTo("New Title"));
            Assert.That(result.Description, Is.EqualTo("New Desc"));
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.UpdatedAt, Is.Not.Null);
            Assert.That(result.CompletedAt, Is.Not.Null);
        }


        [Test]
        public async Task DeleteAsync_RemovesItem_WhenExists()
        {
            // Arrange
            var userId = 1;
            var item = new ToDoItems { Title = "Task", Description = "To Delete", UserId = userId };
            dbContext.ToDoItems.Add(item);
            await dbContext.SaveChangesAsync();

            // Act
            var deletedItem = await repository.DeleteAsync(item.Id, userId);

            // Assert
            Assert.That(deletedItem, Is.Not.Null);
            var exists = await dbContext.ToDoItems.AnyAsync(x => x.Id == item.Id);
            Assert.That(exists, Is.False);
        }


        [TearDown]
        public void Cleanup()
        {
            dbContext?.Dispose();
        }
    }
}
