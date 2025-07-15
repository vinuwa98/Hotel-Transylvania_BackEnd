public class SupervisorComplaintDto
{
    public int ComplaintId { get; set; }
    public string Title { get; set; }
    public string RoomNumber { get; set; } // Or RoomType if you prefer
    public string? CleanerName { get; set; }
}
