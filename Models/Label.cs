namespace TaskPlannerAPI.Models
{
    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Bug", "Feature"
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
