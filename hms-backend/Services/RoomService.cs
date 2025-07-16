using hms_backend.DTOs;
using hms_backend.Services.Interfaces;
using HmsBackend.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace HmsBackend.Services
{
    public class RoomService(AppDbContext appDbContext) : IRoomService
    {
        private readonly AppDbContext _context = appDbContext;

        public async Task<RoomStatusDto> UpdateRoomStatusAsync(string jobId, string jobStatus)
        {
            var roomId = _context.Job
                .Include(j => j.Complaint)
                .Where(j => j.Id == jobId)
                .Select(j => j.Complaint.RoomId)
                .FirstOrDefault();

            if (roomId == null)
                return null;

            var roomStatus = await _context.RoomStatus.FirstOrDefaultAsync(rs => rs.RoomId == roomId);
            if (roomStatus == null)
                return null;

            // Set room status based on job status
            if (jobStatus == "Completed" || jobStatus == "Cancelled" || jobStatus == "Not Fixed")
            {
                roomStatus.Status = "Available";
            }
            else
            {
                roomStatus.Status = "Under Maintenance";
            }

            await _context.SaveChangesAsync();

            return new RoomStatusDto
            {
                RoomId = roomStatus.RoomId,
                Status = roomStatus.Status
            };
        }


    }
}
