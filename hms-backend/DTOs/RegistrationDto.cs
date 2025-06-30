using HmsBackend.Dto;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    public class RegistrationDto
    {
        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string Role { get; set; }

        [Phone]
        public string ContactNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? SupervisorID { get; set; }


    }
}
