using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; }

        public string? UserId { get; set; } //Make it nullable

        public User User { get; set; }

        public List<Complaint> Complaints { get; set; } = new();
    }
}
