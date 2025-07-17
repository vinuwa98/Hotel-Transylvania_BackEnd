using HmsBackend.DTOs;

namespace HmsBackend.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDashboardDto> GetDashboardData();
        Task<List<RoomDto>> GetAllRoomData();
        Task<List<RoomDto>> UpdateRoomStatus(UpdateRoomStatusDto updateRequest);

        Task<RoomStatusDto> UpdateRoomStatusAsync(string jobId, string jobStatus);

        List<string> GetRoomStatusTypes();
    }
}
