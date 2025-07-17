using hms_backend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        IRoomService _roomService = roomService;

        [Authorize(Policy = "HelpDeskOnly")]
        [HttpGet]
        [Route("dashboard-data")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var dashboardData = await _roomService.GetDashboardData();
        
                return Ok(dashboardData);
            }
            catch
        {
                return NoContent();
        }
        }

        [Authorize(Policy = "HelpDeskOnly")]
        [HttpGet]
        [Route("room-status-types")]
        public IActionResult GetRoomStatusTypes()
        {
            try
            {
                var types = _roomService.GetRoomStatusTypes();

                return Ok(types);
            }
            catch
            {
                return NoContent();
            }
            }

        [Authorize(Policy = "HelpDeskOnly")]
        [HttpGet]
        [Route("all-rooms")]
        public async Task<IActionResult> GetAllRoomData()
        {
            try
            {
                var dashboardData = await _roomService.GetAllRoomData();

                return Ok(dashboardData);
            }
            catch
            {
                return NoContent();
            }
        }

        [Authorize(Policy = "HelpDeskOnly")]
        [HttpPost]
        [Route("update-room-status")]
        public async Task<IActionResult> UpdateRoomStatus(UpdateRoomStatusDto updateRequest)
        {
            try
            {
                var updatedData = await _roomService.UpdateRoomStatus(updateRequest);

                return Ok(updatedData);
            }
            catch
            {
                return NoContent();
            }
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
