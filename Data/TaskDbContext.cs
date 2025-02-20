using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Data;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
}
