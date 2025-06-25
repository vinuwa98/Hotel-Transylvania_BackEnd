using HmsBackend.Models;

namespace hms_backend.Models
{
    public class Job
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public  string? Description { get; set; }
        public string? Priority { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }


    }
}
