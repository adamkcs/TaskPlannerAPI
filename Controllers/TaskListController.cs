using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Data;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Controllers
{
    /// <summary>
    /// Controller for managing task lists.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class TaskListController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskListController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all task lists.
        /// </summary>
        /// <returns>List of task lists.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskLists()
        {
            return await _context.TaskLists.Include(t => t.Tasks).ToListAsync();
        }

        /// <summary>
        /// Gets a specific task list by ID.
        /// </summary>
        /// <param name="id">Task list ID</param>
        /// <returns>The task list item.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskList>> GetTaskList(int id)
        {
            var taskList = await _context.TaskLists.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == id);
            if (taskList == null)
                return NotFound();
            return taskList;
        }

        /// <summary>
        /// Creates a new task list.
        /// </summary>
        /// <param name="taskList">Task list item to create</param>
        /// <returns>The created task list.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskList>> CreateTaskList(TaskList taskList)
        {
            _context.TaskLists.Add(taskList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTaskList), new { id = taskList.Id }, taskList);
        }

        /// <summary>
        /// Updates an existing task list.
        /// </summary>
        /// <param name="id">Task list ID</param>
        /// <param name="updatedTaskList">Updated task list object</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskList(int id, TaskList updatedTaskList)
        {
            if (id != updatedTaskList.Id)
                return BadRequest();

            _context.Entry(updatedTaskList).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a task list by ID.
        /// </summary>
        /// <param name="id">Task list ID</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(int id)
        {
            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList == null)
                return NotFound();

            _context.TaskLists.Remove(taskList);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
