using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Dto
{
    public class UserDto
    {
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string Email { get; set; }
    }

}
