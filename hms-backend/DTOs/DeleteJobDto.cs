using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    public class DeleteJobDto
    {
        [Required]
        public required string JobNumber { get; set; }
    }
}
