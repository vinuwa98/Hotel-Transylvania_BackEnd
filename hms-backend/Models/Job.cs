using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(20)]
        public string? Priority { get; set; }

        [Required]
        public int CreatedUserId { get; set; }

        public User CreatedUser { get; set; }

        [Required]
        public int AssignedManagerUserId { get; set; }

        public User AssignedManagereUser { get; set; }

        [Required]
        public int ComplaintId { get; set; }

        public Complaint Complaint { get; set; }

        public List<JobUser> JobUsers { get; set; } = new();
    }
}
