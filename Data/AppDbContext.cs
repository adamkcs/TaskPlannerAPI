﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<TaskLabel> TaskLabels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Many to Many: TaskItem <> Labels
        modelBuilder.Entity<TaskLabel>()
            .HasKey(tl => new { tl.TaskItemId, tl.LabelId });

        modelBuilder.Entity<TaskLabel>()
            .HasOne(tl => tl.TaskItem)
            .WithMany(t => t.TaskLabels)
            .HasForeignKey(tl => tl.TaskItemId);

        modelBuilder.Entity<TaskLabel>()
            .HasOne(tl => tl.Label)
            .WithMany(l => l.TaskLabels)
            .HasForeignKey(tl => tl.LabelId);
    }

}