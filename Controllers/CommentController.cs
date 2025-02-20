using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Data;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Controllers
{
    /// <summary>
    /// Controller for managing comments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>List of comments.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        /// <summary>
        /// Gets a specific comment by ID.
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>The comment item.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();
            return comment;
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">Comment item to create</param>
        /// <returns>The created comment.</returns>
        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <param name="updatedComment">Updated comment object</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, Comment updatedComment)
        {
            if (id != updatedComment.Id)
                return BadRequest();

            _context.Entry(updatedComment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a comment by ID.
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
