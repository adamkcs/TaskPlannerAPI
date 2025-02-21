using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlannerAPI.Data;
using TaskPlannerAPI.Models;

namespace TaskPlannerAPI.Controllers
{
    /// <summary>
    /// Controller for managing boards.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]    public class BoardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BoardController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all boards.
        /// </summary>
        /// <returns>List of boards.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
        {
            return await _context.Boards.Include(b => b.TaskLists).ToListAsync();
        }

        /// <summary>
        /// Gets a specific board by ID.
        /// </summary>
        /// <param name="id">Board ID</param>
        /// <returns>The board item.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
            var board = await _context.Boards.Include(b => b.TaskLists).FirstOrDefaultAsync(b => b.Id == id);
            if (board == null)
                return NotFound();
            return board;
        }

        /// <summary>
        /// Creates a new board.
        /// </summary>
        /// <param name="board">Board item to create</param>
        /// <returns>The created board.</returns>
        [HttpPost]
        public async Task<ActionResult<Board>> CreateBoard(Board board)
        {
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBoard), new { id = board.Id }, board);
        }

        /// <summary>
        /// Updates an existing board.
        /// </summary>
        /// <param name="id">Board ID</param>
        /// <param name="updatedBoard">Updated board object</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(int id, Board updatedBoard)
        {
            if (id != updatedBoard.Id)
                return BadRequest();

            _context.Entry(updatedBoard).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a board by ID.
        /// </summary>
        /// <param name="id">Board ID</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
                return NotFound();

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Gets all boards for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>List of boards.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoardsByUser(string userId)
        {
            var boards = await _context.UserBoards
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.Board)
                .ToListAsync();

            if (!boards.Any())
                return NotFound();

            return Ok(boards);
        }

        /// <summary>
        /// Assigns a user to a board.
        /// </summary>
        /// <param name="boardId">Board ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>No content.</returns>
        [HttpPost("{boardId}/assign/{userId}")]
        public async Task<IActionResult> AssignUserToBoard(int boardId, string userId)
        {
            if (_context.UserBoards.Any(ub => ub.BoardId == boardId && ub.UserId == userId))
                return BadRequest("User is already assigned to this board.");

            _context.UserBoards.Add(new UserBoard { BoardId = boardId, UserId = userId });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Removes a user from a board.
        /// </summary>
        /// <param name="boardId">Board ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>No content.</returns>
        [HttpDelete("{boardId}/unassign/{userId}")]
        public async Task<IActionResult> RemoveUserFromBoard(int boardId, string userId)
        {
            var userBoard = await _context.UserBoards
                .FirstOrDefaultAsync(ub => ub.BoardId == boardId && ub.UserId == userId);

            if (userBoard == null)
                return NotFound();

            _context.UserBoards.Remove(userBoard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
