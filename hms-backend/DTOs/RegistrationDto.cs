using HmsBackend.DTOs;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Filters;

namespace HmsBackend.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class RegistrationDto
    {
        [Required]
        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public required string Role { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public string? ContactNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public string? Address { get; set; }

        public string? SupervisorID { get; set; }
    }

}
