using hms_backend.Models;

namespace HmsBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName {  get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }

        public List<Room> Rooms { get; set; }
        public List<Complaint> Complaints { get; set; }
        public List<Job> Jobs { get; set; }


    }
}
