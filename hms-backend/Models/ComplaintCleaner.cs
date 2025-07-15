using global::HmsBackend.Models;
// Create a new model for the assignment table
// File: Models/ComplaintCleaner.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HmsBackend.Models
{
    public class ComplaintCleaner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ComplaintId { get; set; }

        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; }

        [Required]
        public string CleanerId { get; set; }

        [ForeignKey("CleanerId")]
        public User Cleaner { get; set; }
    }
}
