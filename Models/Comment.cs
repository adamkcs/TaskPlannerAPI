namespace TaskPlannerAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;
    }
}
