namespace HmsBackend.DTOs
{
    public class RoomDashboardDto
    {
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int TotalSingleRooms { get; set; }
        public int AvailableSingleRooms { get; set; }
        public int TotalDoubleRooms { get; set; }
        public int AvailableDoubleRooms { get; set; }
        public int TotalTwinRooms { get; set; }
        public int AvailableTwinRooms { get; set; }
        public int TotalSuiteRooms { get; set; }
        public int AvailableSuiteRooms { get; set; }
        public int TotalDeluxeRooms { get; set; }
        public int AvailableDeluxeRooms { get; set; }
        public int TotalFamilyRooms { get; set; }
        public int AvailableFamilyRooms { get; set; }
        public int RoomsUnderMaintenance { get; set; }
    }
}
