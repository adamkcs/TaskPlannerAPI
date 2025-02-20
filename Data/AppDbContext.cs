using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Data;

/// <summary>
/// Database context using SQLite.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
    public DbSet<Board> Boards { get; set; }
}