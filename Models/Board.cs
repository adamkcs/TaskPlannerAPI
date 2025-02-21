namespace TaskPlannerAPI.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<TaskList> TaskLists { get; set; } = new();
        public ICollection<UserBoard> UserBoards { get; set; }
    }
}
