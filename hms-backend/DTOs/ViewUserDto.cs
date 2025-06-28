using System.ComponentModel.DataAnnotations;

namespace hms_backend.DTOs
{
    public class ViewUserDto
    {
       
        public string UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? SupervisorID { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }


    }
}
