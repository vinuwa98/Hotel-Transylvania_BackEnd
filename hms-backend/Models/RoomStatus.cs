using HmsBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HmsBackend.Models
{
    public class RoomStatus
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public required string RoomId { get; set; }

        [ForeignKey("RoomId")]
        public required Room Room { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}
