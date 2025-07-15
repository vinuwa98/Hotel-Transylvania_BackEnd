using System.ComponentModel.DataAnnotations;

namespace HmsBackend.DTOs
{
    public class UpdateRoomStatusDto
    {
        [Required]
        public required string RoomNumber { get; set; }

        [Required]
        public required string NewStatus { get; set; }
    }
}
