using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class User : IdentityUser
    {
        internal string FullName;
        internal string Status;
        public required string Role { get; set; }

        public bool IsActive { get; set; } = true;

        //public int id { get; set; }

        // limit to 50 characters

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

        [Required]
        public required string Status { get; set; }

        public List<Room>? Rooms { get; set; } = new();
        public List<Complaint>? Complaints { get; set; } = new();
        public List<JobUser>? JobUsers { get; set; } = new();
    }
}
