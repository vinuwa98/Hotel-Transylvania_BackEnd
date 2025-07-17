using HmsBackend.Models;

namespace HmsBackend.DTOs
{
    public class UpdateJobUsersDto
    {
        public string JobId { get; set; }
        public List<string> UserIds { get; set; } = new();
    }
}
