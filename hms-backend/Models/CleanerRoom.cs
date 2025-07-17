namespace hms_backend.Models
{
    using global::HmsBackend.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace HmsBackend.Models
    {
        public class CleanerRoom
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public int RoomId { get; set; }
            public Room Room { get; set; }

            [Required]
            public string CleanerId { get; set; }
            public User Cleaner { get; set; }

            public DateTime AssignedDate { get; set; } = DateTime.Now;
        }
    }
}
