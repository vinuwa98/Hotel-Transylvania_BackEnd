using HmsBackend.DTOs;
using HmsBackend.DTOs;

namespace HmsBackend.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDashboardDto> GetDashboardData();
        Task<List<RoomDto>> GetAllRoomData();
        Task<List<RoomDto>> UpdateRoomStatus(UpdateRoomStatusDto updateRequest);
    }
}
