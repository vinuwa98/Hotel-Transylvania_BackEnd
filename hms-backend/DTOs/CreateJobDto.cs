using System.ComponentModel.DataAnnotations;

namespace hms_backend.DTOs
{
    public class CreateJobDto
    {
        [Required]
        public required string ComplaintNumber { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Priority { get; set; }
    }
}
