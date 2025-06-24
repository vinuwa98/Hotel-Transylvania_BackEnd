using System.ComponentModel.DataAnnotations;

namespace HmsBackend.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public required string RoomType {  get; set; }
    }
}
