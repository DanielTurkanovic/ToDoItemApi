using Microsoft.EntityFrameworkCore;

namespace ToDoItemApi.Models.Domain
{
    [Index(nameof(UserId), nameof(Title), IsUnique = true)]
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsCompleted { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // FK on User
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
