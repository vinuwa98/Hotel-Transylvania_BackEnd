using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    public class SupervisorInfoDto
    {
        [Required]
        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public required string SupervisorId { get; set; }
    }
}
