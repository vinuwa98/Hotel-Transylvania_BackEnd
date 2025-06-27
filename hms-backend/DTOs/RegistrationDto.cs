using HmsBackend.Dto;
using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class RegistrationDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        public string? SupervisorID { get; set; }
        public string? ContactNumber { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Role { get; set; }
    }

}
