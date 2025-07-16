using hms_backend.Services.Interfaces;
using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Services;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HmsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        private readonly AppDbContext _context;

        public ComplaintController(IComplaintService complaintService, AppDbContext context)
        {
            _complaintService = complaintService;
            _context = context;
        }


        //public ComplaintController(AppDbContext context)
        //{
        //    _context = context;
        //}

        [Authorize(Roles = "Supervisor")]
        [HttpGet("supervisor-complaints/{supervisorId}")]
        public async Task<IActionResult> GetComplaintsBySupervisor(string supervisorId)
        {
            var complaints = await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Room)
                .Include(c => c.Jobs)
                    .ThenInclude(j => j.Cleaner)
                .Where(c => c.User.SupervisorID == supervisorId && c.IsActive)
                .ToListAsync();

            var result = complaints
                .Select(c => new ViewComplaintDto
                {
                    ComplaintId = c.Id,
                    Title = c.Title,
                    RoomNumber = c.Room.RoomId.ToString(),
                    CleanerName = c.Jobs
                        .SelectMany(j => j.JobUsers)
                        .Select(ju => ju.User.FirstName + " " + ju.User.LastName)
                        .FirstOrDefault() ?? "Not Assigned"
                })
                .ToList();

            return Ok(result);
        }

        [Authorize(Roles = "Supervisor")]
        [HttpPut("deactivate/{complaintId}")]
        public async Task<IActionResult> DeactivateComplaint(int complaintId)
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            if (complaint == null || !complaint.IsActive)
            {
                return NotFound(new { message = "Complaint not found or already inactive." });
            }

            complaint.IsActive = false;
            _context.Complaints.Update(complaint);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Complaint deactivated successfully." });
        }

        [Authorize(Roles = "Cleaner,Supervisor")]
        [HttpPost("add-complaint")]
        public async Task<IActionResult> AddComplaint([FromForm] ComplaintDto complaintDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            

            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            
            var result = await _complaintService.AddComplaintAsync(complaintDto, userId);

            if (result)
            {
                return Ok(new { message = "Complaint submitted successfully." });
            }

            return BadRequest(new { message = "Failed to submit complaint." });
        }

        
    }
}
