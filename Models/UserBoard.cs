namespace TaskPlannerAPI.Models
{
    public class UserBoard
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BoardId { get; set; }

        public ApplicationUser User { get; set; }  // Assuming you have an AppUser entity
        public Board Board { get; set; }
    }
}
