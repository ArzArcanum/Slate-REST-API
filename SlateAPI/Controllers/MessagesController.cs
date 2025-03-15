using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlateAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SlateAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly SlateDbContext _context;

        public MessagesController(SlateDbContext context)
        {
            _context = context;
        }

        // GET: /Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        // GET: /Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(long id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null) // Return 404 if message is null (instead of 204)
            {
                return NotFound();
            }

            return message;
        }

        // POST: /Messages
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(MessageRequestDTO messageDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = new Message
            {
                UserId = userId,
                Content = messageDTO.Content
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        // DELETE: /Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
