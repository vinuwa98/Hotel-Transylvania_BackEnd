using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class Room
    {
        //[Key]
        public string RoomId { get; set; }

        [Required]
        public required string RoomNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; }

        //[Required]
        public string? UserId { get; set; }

        public User User { get; set; }

        public List<Complaint> Complaints { get; set; } = new();
    }
}
