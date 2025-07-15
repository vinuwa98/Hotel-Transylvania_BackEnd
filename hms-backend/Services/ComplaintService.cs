using HmsBackend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Services
{
    public class ComplaintService(AppDbContext appDbContext) : IComplaintsService
    {
        private readonly AppDbContext _context = appDbContext;

        public async Task<List<ComplaintDto>> GetAllComplaints()
        {
            try
            {
                var complaints = await (
                    from complaint in _context.Complaints
                    select new ComplaintDto
                    {
                        ComplaintNumber = complaint.ComplaintNumber,
                        Title = complaint.Title,
                        DateTime = complaint.DateTime,
                        Description = complaint.Description,
                        ImgUrl = complaint.ImgUrl,
                        RoomNumber = complaint.Room.RoomNumber
                    }).ToListAsync();

                return complaints;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Gathering all complaints failed!", ex);
            }
        }
    }
}
