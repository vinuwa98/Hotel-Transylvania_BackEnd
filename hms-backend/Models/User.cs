using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class User : IdentityUser
    {
        //public string? Status { get; set; }

        public bool IsActive { get; set; } = true;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [Phone]
        public string? ContactNumber { get; set; }

        public string? SupervisorID { get; set; }

        [Required]
        public required string Role { get; set; }

        public List<Room>? Rooms { get; set; } = new();
        public List<Complaint>? Complaints { get; set; } = new();
        public List<JobUser>? JobUsers { get; set; } = new();
    }
}
