using hms_backend.Models.HmsBackend.Models;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController(AppDbContext appDbContext) : ControllerBase
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        [HttpGet]
        [Route("all-rooms")]
        [ProducesResponseType(typeof(IList<Room>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Room>>> GetRooms()
        {
            try
            {
                return await _appDbContext.Rooms.ToListAsync();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("new-room")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> CreateRoom(Room room)
        {
            try
            {
                if (room == null) return BadRequest();

                _appDbContext.Rooms.Add(room);
                await _appDbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Supervisor")]
        [HttpPost("assign-room")]
        public async Task<IActionResult> AssignRoom([FromBody] AssignRoomDto dto)
        {
            var exists = await _appDbContext.CleanerRooms
                .AnyAsync(x => x.RoomId == dto.RoomId && x.CleanerId == dto.CleanerId);

            if (exists)
                return BadRequest("Cleaner already assigned to this room.");

            var assign = new CleanerRoom
            {
                RoomId = dto.RoomId,
                CleanerId = dto.CleanerId
            };

            _appDbContext.CleanerRooms.Add(assign);
            await _appDbContext.SaveChangesAsync();

            return Ok("Cleaner assigned to room successfully.");
        }

        [Authorize(Roles = "Supervisor")]
        [HttpGet("room-assignments")]
        public async Task<IActionResult> GetRoomAssignments()
        {
            var assignments = await _appDbContext.CleanerRooms
                .Include(cr => cr.Room)
                .Include(cr => cr.Cleaner)
                .ToListAsync();

            var result = assignments
                .GroupBy(cr => cr.RoomId)
                .Select(g => new
                {
                    RoomId = g.Key,
                    RoomType = g.First().Room.RoomType,
                    CleanerNames = g.Select(c => c.Cleaner.FirstName + " " + c.Cleaner.LastName).ToList()
                });

            return Ok(result);
        }
    }
}
