namespace HmsBackend.DTOs
{
    public class ComplaintDto
    {
        //public string Title { get; set; }
        //public DateTime DateTime { get; set; }
        //public string Description { get; set; }
        //public string? ImgBase64 { get; set; }
        //public int RoomId { get; set; }
        //public string UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public IFormFile? Image { get; set; }

    }
}
