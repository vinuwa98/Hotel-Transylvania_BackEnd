namespace hms_backend.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int TotalWorkers { get; set; }

        public int ActiveJobs { get; set; }
    }
}
