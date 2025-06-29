namespace hms_backend.DTOs
{
    public class UserViewDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; }
    }
}
