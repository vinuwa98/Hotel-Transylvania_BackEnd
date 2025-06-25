using hms_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public required string RoomType {  get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        
        public List<Complaint> Complaints { get; set; }
    }
}
