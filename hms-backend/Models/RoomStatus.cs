using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HmsBackend.Models
{
    public class RoomStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public required Room Room { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}
