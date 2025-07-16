using HmsBackend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hms_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("get-all-rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();

            var result = rooms.Select(r => new
            {
                roomId = r.RoomId,
                roomType = r.RoomType
            });

            return Ok(result);
        }

    }
}
