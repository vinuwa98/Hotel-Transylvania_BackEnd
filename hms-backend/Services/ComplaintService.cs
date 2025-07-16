using hms_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;


namespace HmsBackend.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly AppDbContext _context;

        public ComplaintService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddComplaintAsync(ComplaintDto dto, string userId)
        {
            var complaint = new Complaint
            {
                ComplaintCode = GenerateComplaintCode(),
                Title = dto.Title,
                Description = dto.Description,
                RoomId = dto.RoomId,
                UserId = userId,
                DateTime = DateTime.UtcNow,
                IsActive = true
            };

          
            
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                
                complaint.ImagePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
            }


            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateComplaintCode()
        {
            // Example: C00001
            int count = _context.Complaints.Count() + 1;
            return $"C{count.ToString("D5")}";
        }

       
    }
}
