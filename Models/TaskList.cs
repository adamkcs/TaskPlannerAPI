namespace TaskPlannerAPI.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "To Do", "In Progress", "Done"
        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;
        public List<TaskItem> TaskItems { get; set; } = new();
    }
}
