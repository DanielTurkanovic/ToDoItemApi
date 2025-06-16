namespace ToDoItemApi.Models.DTO
{
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
