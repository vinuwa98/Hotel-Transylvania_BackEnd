using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Url]
        public string? ImgUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int RoomId { get; set; }

        public Room Room { get; set; }

        public List<Job> Jobs { get; set; } = new();

        public bool IsActive { get; set; } = true; // default is active

    }
}
