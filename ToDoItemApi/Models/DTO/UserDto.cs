namespace ToDoItemApi.Models.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
