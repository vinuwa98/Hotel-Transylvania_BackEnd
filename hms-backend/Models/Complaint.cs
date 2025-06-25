using HmsBackend.Models;

namespace hms_backend.Models
{
    public class Complaint
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime DateTime { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }

        public List<Job> Jobs { get; set; }


    }
}
