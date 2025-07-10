namespace ToDoItemApi.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Store the hashed password for security
        public bool IsAdmin { get; set; } 
        public bool IsDeleted { get; set; } = false;

        public ICollection<ToDoItem> ToDoItems { get; set; } // Navigation property for related ToDo items
    }
}
