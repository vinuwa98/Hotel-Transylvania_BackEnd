using HmsBackend.DTOs;
using HmsBackend.Models;

namespace hms_backend.Services.Interfaces
{
    public interface IComplaintService
    {
        Task<bool> AddComplaintAsync(ComplaintDto complaintDto, string userId);

    }
}
