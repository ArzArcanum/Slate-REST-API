using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API.Persistence;
using API.Entities;
using API.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Primitives;

namespace API.Controllers
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
            return await _context.Messages
                .Include(m => m.User)
                .ToListAsync();
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
            if (!Request.Headers.TryGetValue("ID-Token", out StringValues idTokenHeader))
            {
                return BadRequest("ID Token is required.");
            }

            if (string.IsNullOrEmpty(idTokenHeader))
            {
                return BadRequest("ID Token is empty.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(idTokenHeader);
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(idTokenHeader);

            var username = token?.Claims?.FirstOrDefault(c => c.Type == "nickname")?.Value;

            Console.Write(username);
            // Check if user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

            // If user doesn't exist, create a new entry
            if (user == null)
            {
                // Extract ID token


                user = new User
                {
                    Id = userId,
                    Username = username
                };

                _context.Users.Add(user);
            }

            // Create new message entry
            var message = new Message
            {
                Content = messageDTO.Content,
                CreatedAt = DateTime.UtcNow,
                User = user // Link to author user
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
