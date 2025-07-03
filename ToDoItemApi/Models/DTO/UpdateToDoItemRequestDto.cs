namespace ToDoItemApi.Models.DTO
{
    public class UpdateToDoItemRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; } 
    }
}
