using hms_backend.DTOs;

namespace hms_backend.Services.Interfaces
{
    public interface IRoomService
    {

        Task<RoomStatusDto> UpdateRoomStatusAsync(string jobId, string jobStatus);

    }
}
