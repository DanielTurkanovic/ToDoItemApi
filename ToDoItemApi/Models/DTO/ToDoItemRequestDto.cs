namespace ToDoItemApi.Models.DTO
{
    public class ToDoItemRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; } 
    }
}
