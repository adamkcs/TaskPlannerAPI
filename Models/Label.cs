namespace TaskPlannerAPI.Models
{
    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Bug", "Feature"
        public int BoardId { get; set; }
    }
}
