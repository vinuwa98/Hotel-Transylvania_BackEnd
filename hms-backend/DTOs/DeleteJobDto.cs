using System.ComponentModel.DataAnnotations;

namespace hms_backend.DTOs
{
    public class DeleteJobDto
    {
        [Required]
        public required string JobNumber { get; set; }
    }
}
