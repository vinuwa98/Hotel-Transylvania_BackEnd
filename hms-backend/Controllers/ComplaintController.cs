using HmsBackend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController(IComplaintsService complaintsService) : ControllerBase
    {
        //private readonly AppDbContext _context;
        private readonly IComplaintsService _complaintsService = complaintsService;

        //public ComplaintController(AppDbContext context)
        //{
        //    _context = context;
        //}

        [Authorize(Policy = "HelpDeskOnly")]
        [Route("all-complaints")]
        [HttpGet]
        public async Task<IActionResult> GetAllComplaints()
        {
            try
            {
                var allComplaints = await _complaintsService.GetAllComplaints();

                return Ok(allComplaints);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[Authorize(Roles = "Supervisor")]
        //[HttpGet("supervisor-complaints/{supervisorId}")]
        //public async Task<IActionResult> GetComplaintsBySupervisor(string supervisorId)
        //{
        //    // Step 1: Get complaints for supervisor
        //    var complaints = await _context.Complaints
        //        .Include(c => c.User)
        //        .Include(c => c.Room)
        //        .Where(c => c.User.SupervisorID == supervisorId && c.IsActive)
        //        .ToListAsync();

        //    // Step 2: Get all complaint IDs
        //    var complaintIds = complaints.Select(c => c.Id).ToList();

        //    // Step 3: Get all related cleaners in one query
        //    var complaintCleaners = await _context.ComplaintCleaners
        //        .Where(cc => complaintIds.Contains(cc.ComplaintId))
        //        .Include(cc => cc.Cleaner)
        //        .ToListAsync();

        //    // Step 4: Group cleaners by complaint ID
        //    var cleanerMap = complaintCleaners
        //        .GroupBy(cc => cc.ComplaintId)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Select(cc => $"{cc.Cleaner.FirstName} {cc.Cleaner.LastName}").ToList()
        //        );

        //    // Step 5: Map to DTOs
        //    var result = complaints.Select(c => new ViewComplaintDto
        //    {
        //        ComplaintId = c.Id,
        //        Title = c.Title,
        //        RoomNumber = c.Room?.RoomId.ToString() ?? "Unknown",
        //        CleanerName = cleanerMap.ContainsKey(c.Id)
        //            ? string.Join(", ", cleanerMap[c.Id])
        //            : "Not Assigned"
        //    }).ToList();

        //    return Ok(result);
        //}



        //[Authorize(Roles = "Supervisor")]
        //[HttpPut("deactivate/{complaintId}")]
        //public async Task<IActionResult> DeactivateComplaint(int complaintId)
        //{
        //    var complaint = await _context.Complaints.FindAsync(complaintId);
        //    if (complaint == null || !complaint.IsActive)
        //    {
        //        return NotFound(new { message = "Complaint not found or already inactive." });
        //    }

        //    complaint.IsActive = false;
        //    _context.Complaints.Update(complaint);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "Complaint deactivated successfully." });
        //}

        //[Authorize(Roles = "Supervisor")]
        //[HttpPost("assign-cleaner")]
        //public async Task<IActionResult> AssignCleaner([FromBody] AssignCleanerDto dto)
        //{
        //    var exists = await _context.ComplaintCleaners
        //        .AnyAsync(cc => cc.ComplaintId == dto.ComplaintId && cc.CleanerId == dto.CleanerId);

        //    if (exists)
        //        return BadRequest("Cleaner already assigned to this complaint.");

        //    var newAssignment = new ComplaintCleaner
        //    {
        //        ComplaintId = dto.ComplaintId,
        //        CleanerId = dto.CleanerId
        //    };

        //    _context.ComplaintCleaners.Add(newAssignment);
        //    await _context.SaveChangesAsync();

        //    return Ok("Cleaner assigned successfully.");
        //}

    }
}
