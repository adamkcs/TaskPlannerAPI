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

        /// <summary>
        /// Gets all tasks within a specific task list, with optional filters.
        /// </summary>
        /// <param name="listId">The ID of the task list.</param>
        /// <param name="status">Optional filter: "completed" or "pending".</param>
        /// <returns>List of tasks.</returns>
        [HttpGet("{listId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByList(int listId, [FromQuery] string status = null)
        {
            var query = _context.Tasks.Where(t => t.TaskListId == listId);

            if (status == "completed")
                query = query.Where(t => t.IsCompleted);
            else if (status == "pending")
                query = query.Where(t => !t.IsCompleted);

            var tasks = await query.ToListAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Moves a task from one task list to another.
        /// </summary>
        /// <param name="taskId">The ID of the task to move.</param>
        /// <param name="newListId">The ID of the destination task list.</param>
        /// <returns>Updated task details.</returns>
        [HttpPut("move-task/{taskId}/to/{newListId}")]
        public async Task<IActionResult> MoveTask(int taskId, int newListId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return NotFound("Task not found.");

            task.TaskListId = newListId;
            await _context.SaveChangesAsync();
            return Ok(task);
        }

        /// <summary>
        /// Gets the total number of tasks per task list.
        /// </summary>
        /// <returns>List of task counts per list.</returns>
        [HttpGet("task-count")]
        public async Task<ActionResult<IEnumerable<object>>> GetTaskCountsPerList()
        {
            var counts = await _context.TaskLists
                .Select(l => new
                {
                    l.Id,
                    l.Name,
                    TaskCount = _context.Tasks.Count(t => t.TaskListId == l.Id)
                })
                .ToListAsync();

            return Ok(counts);
        }

        /// <summary>
        /// Gets the ratio of completed to pending tasks in a specific task list.
        /// </summary>
        /// <param name="listId">The ID of the task list.</param>
        /// <returns>Completion ratio.</returns>
        [HttpGet("{listId}/completion-ratio")]
        public async Task<ActionResult<object>> GetCompletionRatio(int listId)
        {
            var totalTasks = await _context.Tasks.CountAsync(t => t.TaskListId == listId);
            var completedTasks = await _context.Tasks.CountAsync(t => t.TaskListId == listId && t.IsCompleted);

            if (totalTasks == 0)
                return Ok(new { CompletionRatio = "N/A (No Tasks)" });

            double ratio = (double)completedTasks / totalTasks;
            return Ok(new { CompletionRatio = ratio.ToString("P1") });
        }
    }
}
