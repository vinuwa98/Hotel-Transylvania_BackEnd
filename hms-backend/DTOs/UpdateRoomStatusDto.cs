using System.ComponentModel.DataAnnotations;

namespace hms_backend.DTOs
{
    public class UpdateRoomStatusDto
    {
        [Required]
        public required string RoomNumber { get; set; }

        [Required]
        public required string NewStatus { get; set; }
    }
}
