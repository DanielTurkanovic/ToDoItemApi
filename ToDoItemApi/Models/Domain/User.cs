namespace ToDoItemApi.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
