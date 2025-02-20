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
    }
}