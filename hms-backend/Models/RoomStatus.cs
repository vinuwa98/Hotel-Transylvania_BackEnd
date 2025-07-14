using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace hms_backend.Models
{
    public class RoomStatus
    {
        public int Id { get; set; }

        [Required]
        public required Room Room { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}
