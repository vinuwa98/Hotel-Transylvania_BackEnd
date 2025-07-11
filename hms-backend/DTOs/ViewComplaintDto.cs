namespace HmsBackend.DTOs
{
    public class ViewComplaintDto
    {
        public int ComplaintId { get; set; }
        public string Title { get; set; }
        public string RoomNumber { get; set; }
        public string CleanerName { get; set; } = "Not Assigned";
    }
}
