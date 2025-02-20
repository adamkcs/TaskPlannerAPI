using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Data;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Controllers
{
    /// <summary>
    /// Controller for managing labels.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LabelController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all labels.
        /// </summary>
        /// <returns>List of labels.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Label>>> GetLabels()
        {
            return await _context.Labels.ToListAsync();
        }

        /// <summary>
        /// Gets a specific label by ID.
        /// </summary>
        /// <param name="id">Label ID</param>
        /// <returns>The label item.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Label>> GetLabel(int id)
        {
            var label = await _context.Labels.FindAsync(id);
            if (label == null)
                return NotFound();
            return label;
        }

        /// <summary>
        /// Creates a new label.
        /// </summary>
        /// <param name="label">Label item to create</param>
        /// <returns>The created label.</returns>
        [HttpPost]
        public async Task<ActionResult<Label>> CreateLabel(Label label)
        {
            _context.Labels.Add(label);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLabel), new { id = label.Id }, label);
        }

        /// <summary>
        /// Updates an existing label.
        /// </summary>
        /// <param name="id">Label ID</param>
        /// <param name="updatedLabel">Updated label object</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(int id, Label updatedLabel)
        {
            if (id != updatedLabel.Id)
                return BadRequest();

            _context.Entry(updatedLabel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a label by ID.
        /// </summary>
        /// <param name="id">Label ID</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            var label = await _context.Labels.FindAsync(id);
            if (label == null)
                return NotFound();

            _context.Labels.Remove(label);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
