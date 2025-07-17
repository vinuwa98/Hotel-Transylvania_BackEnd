using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HmsBackend.Models
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public required string JobNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public required bool IsDeleted { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(20)]
        public string? Priority { get; set; }

        //[Required]
        public string? CreatedUserId { get; set; }

        public User? CreatedUser { get; set; }

        public string? AssignedManagerUserId { get; set; }

        public User? AssignedManagerUser { get; set; }

        [Required]
        public string ComplaintId { get; set; }

        public Complaint Complaint { get; set; }

        public List<JobUser> JobUsers { get; set; } = new();
    }
}
