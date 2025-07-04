using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
