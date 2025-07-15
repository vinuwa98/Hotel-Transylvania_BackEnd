using HmsBackend.DTOs;

namespace HmsBackend.Services.Interfaces
{
    public interface IComplaintsService
    {
        Task<List<ComplaintDto>> GetAllComplaints();
    }
}
