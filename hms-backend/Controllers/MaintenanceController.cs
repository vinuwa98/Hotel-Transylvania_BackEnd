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
    }
}
