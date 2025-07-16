using hms_backend.DTOs;
using hms_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hms_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPut("update-status/{jobId}")]
        public async Task<IActionResult> UpdateStatus(string jobId, [FromBody] RoomStatusUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(jobId) || string.IsNullOrWhiteSpace(request?.JobStatus))
            {
                return BadRequest("JobId and JobStatus are required.");
            }

            var updatedStatus = await _roomService.UpdateRoomStatusAsync(jobId, request.JobStatus);

            if (updatedStatus == null)
            {
                return NotFound("Room or status not found for the given JobId");
            }

            return Ok(updatedStatus);
        }



    }
}
