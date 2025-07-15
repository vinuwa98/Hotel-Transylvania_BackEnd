using HmsBackend.Models;

namespace hms_backend.DTOs
{
    public class UpdateJobUsersDto
    {
        public string JobId { get; set; }
        public List<string> UserIds { get; set; } = new();
    }
}
