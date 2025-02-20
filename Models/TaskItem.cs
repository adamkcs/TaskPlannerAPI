namespace TaskPlannerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; } // 1 = High, 2 = Medium, 3 = Low
        public string Status { get; set; } = "To Do"; // Can be updated dynamically
        public int TaskListId { get; set; }
        public TaskList TaskList { get; set; } = null!; // Navigation Property
        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Label>? Labels { get; set; }
        public ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
    }
}