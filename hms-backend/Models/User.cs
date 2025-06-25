using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    
    public class User :IdentityUser
    {
        //[Key]
        //public int id { get; set; }

        [Required]
        [MaxLength(50)] // limit to 50 characters
        public string UserName { get; set; }

        [Required]
        [MinLength(6)] // enforce minimum password length
        public string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [Phone]
        public string? ContactNumber { get; set; }

        public int? SupervisorID { get; set; }

      
        public List<Room> Rooms { get; set; } = new();
        public List<Complaint> Complaints { get; set; } = new();
        public List<JobUser> JobUsers { get; set; } = new();
    }
}
