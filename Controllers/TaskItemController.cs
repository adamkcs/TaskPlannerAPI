using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Data;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Controllers
{
    /// <summary>
    /// Controller for managing task items.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all TaskItems.
        /// </summary>
        /// <returns>List of TaskItems.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
            return await _context.TaskItems.Include(t => t.Labels).Include(t => t.Comments).ToListAsync();
        }

        /// <summary>
        /// Gets a specific task by ID.
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>The task item.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.TaskItems.Include(t => t.Labels).Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return NotFound();
            return task;
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">Task item to create</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="updatedTask">Updated task object</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem updatedTask)
        {
            if (id != updatedTask.Id)
                return BadRequest();

            _context.Entry(updatedTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Moves a task to another list.
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="newTaskListId">New task list ID</param>
        /// <returns>No content.</returns>
        [HttpPatch("{id}/move")]
        public async Task<IActionResult> MoveTaskToList(int id, [FromBody] int newTaskListId)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound();

            task.TaskListId = newTaskListId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Updates the status of a task.
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="newStatus">New task status</param>
        /// <returns>No content.</returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskItemstatus(int id, [FromBody] string newStatus)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound();

            task.Status = newStatus;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Gets tasks based on filters like priority, completion status, and due dates.
        /// </summary>
        /// <param name="status">Optional: "completed", "pending".</param>
        /// <param name="priority">Optional: "low", "medium", "high".</param>
        /// <param name="dueDate">Optional: Only tasks due before this date.</param>
        /// <returns>List of filtered tasks.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetFilteredTasks(
            [FromQuery] string status = null,
            [FromQuery] int priority = 0,
            [FromQuery] DateTime? dueDate = null)
        {
            var query = _context.TaskItems.AsQueryable();

            if (status == "completed")
                query = query.Where(t => t.IsCompleted);
            else if (status == "pending")
                query = query.Where(t => !t.IsCompleted);

            if (priority > 0)
                query = query.Where(t => t.Priority == priority);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate <= dueDate);

            var tasks = await query.ToListAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Updates the status of multiple tasks in bulk.
        /// </summary>
        /// <param name="taskIds">List of task IDs to update.</param>
        /// <param name="isCompleted">New completion status.</param>
        /// <returns>No content.</returns>
        [HttpPut("bulk-update-status")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] List<int> taskIds, [FromQuery] bool isCompleted)
        {
            var tasks = await _context.TaskItems.Where(t => taskIds.Contains(t.Id)).ToListAsync();

            if (!tasks.Any())
                return NotFound("No tasks found.");

            foreach (var task in tasks)
                task.IsCompleted = isCompleted;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Assigns a dependency between tasks (Task B depends on Task A).
        /// </summary>
        /// <param name="taskItemId">The dependent task.</param>
        /// <param name="dependencyId">The task it depends on.</param>
        /// <returns>Updated task details.</returns>
        [HttpPut("{taskId}/depends-on/{dependencyId}")]
        public async Task<IActionResult> SetTaskItemDependency(int taskItemId, int dependencyId)
        {
            var task = await _context.TaskItems.FindAsync(taskItemId);
            var dependency = await _context.TaskItems.FindAsync(dependencyId);

            if (task == null || dependency == null)
                return NotFound("Task or dependency not found.");

            task.DependencyTaskItemId = dependencyId;
            await _context.SaveChangesAsync();
            return Ok(task);
        }

        /// <summary>
        /// Retrieves all overdue tasks.
        /// </summary>
        /// <returns>List of overdue tasks.</returns>
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetOverdueTasks()
        {
            var today = DateTime.UtcNow;
            var overdueTasks = await _context.TaskItems
                .Where(t => !t.IsCompleted && t.DueDate < today)
                .ToListAsync();

            return Ok(overdueTasks);
        }

        /// <summary>
        /// Gets all tasks assigned to a specific user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>List of tasks assigned to the user.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByUser(string userId)
        {
            var tasks = await _context.TaskItems
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();

            return Ok(tasks);
        }
    }
}