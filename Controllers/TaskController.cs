using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Data;
using TaskPlannerAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskPlannerAPI.Controllers;

/// <summary>
/// API controller for managing tasks.
/// </summary>
[Route("api/tasks")]
[ApiController]
[Authorize] // All endpoints
public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    /// <returns>List of tasks.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        return await (_context.Tasks?.ToListAsync() ?? Task.FromResult(new List<TaskItem>()));
    }

    /// <summary>
    /// Retrieves a specific task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The requested task.</returns>
    /// <response code="200">Returns the requested task.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        if (_context.Tasks == null)
        {
            return Problem("Tasks table is not available.");
        }

        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        return task;
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="task">Task object to create.</param>
    /// <returns>The created task.</returns>
    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
    {
        if (_context.Tasks == null)
        {
            return Problem("Tasks table is not available.");
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">Task ID.</param>
    /// <param name="task">Updated task object.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskItem task)
    {
        if (id != task.Id)
            return BadRequest();

        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes a task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>No content on success.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        if (_context.Tasks == null)
        {
            return Problem("Tasks table is not available.");
        }

        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}