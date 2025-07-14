using hms_backend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Services
{
    public class RoomService(AppDbContext appDbContext) : IRoomService
    {
        private readonly AppDbContext _context = appDbContext;

        public async Task<RoomDashboardDto> GetDashboardData()
        {
            try
            {
                var result = new RoomDashboardDto
                {
                    TotalRooms = await _context.Rooms.CountAsync(),
                    AvailableRooms = await _context.RoomStatus.CountAsync(r => r.Status == "Available"),
                    TotalSingleRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Single"),
                    AvailableSingleRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Single" && r.Status == "Available"),
                    TotalDoubleRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Double"),
                    AvailableDoubleRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Double" && r.Status == "Available"),
                    TotalTwinRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Twin"),
                    AvailableTwinRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Twin" && r.Status == "Available"),
                    TotalSuiteRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Suite"),
                    AvailableSuiteRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Suite" && r.Status == "Available"),
                    TotalDeluxeRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Deluxe"),
                    AvailableDeluxeRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Deluxe" && r.Status == "Available"),
                    TotalFamilyRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Family"),
                    AvailableFamilyRooms = await _context.RoomStatus.CountAsync(r => r.Room.RoomType == "Family" && r.Status == "Available"),
                    RoomsUnderMaintenance = await _context.RoomStatus.CountAsync(r => r.Status == "Maintenance"),
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Gathering room dashboard data failed: ", ex);
            }
        }

        public async Task<List<RoomDto>> GetAllRoomData()
        {
            try
            {
                var result = await (from room in _context.RoomStatus
                                    select new RoomDto
                                    {
                                        RoomNumber = room.Room.RoomNumber,
                                        RoomStatus = room.Status,
                                        RoomType = room.Room.RoomType,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Gathering room dashboard data failed: ", ex);
            }
        }

        public async Task<List<RoomDto>> UpdateRoomStatus(UpdateRoomStatusDto updateRequest)
        {
            try
            {
                var _targetRoom = await (from room in _context.RoomStatus where room.Room.RoomNumber == updateRequest.RoomNumber select room).FirstOrDefaultAsync();

                if (_targetRoom == null)
                    throw new InvalidOperationException($"Room with number = {updateRequest.RoomNumber} cannot find");

                _targetRoom.Status = updateRequest.NewStatus;

                await _context.SaveChangesAsync();

                var result = await (from room in _context.RoomStatus
                                    select new RoomDto
                                    {
                                        RoomNumber = room.Room.RoomNumber,
                                        RoomStatus = room.Status,
                                        RoomType = room.Room.RoomType,
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Updating room data failed: ", ex);
            }
        }
    }
}
