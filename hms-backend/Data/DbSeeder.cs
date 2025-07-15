using HmsBackend;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, UserManager<User> userManager)
    {
        if (!context.Complaints.Any())
        {
            // Get a supervisor user
            var supervisor = await userManager.Users.FirstOrDefaultAsync(u => u.Role == "Supervisor");
            var room = await context.Rooms.FirstOrDefaultAsync(); // Use first room

            if (supervisor != null && room != null)
            {
                var complaint1 = new Complaint
                {
                    Title = "Leaking Tap",
                    Description = "Bathroom tap is leaking heavily",
                    DateTime = DateTime.Now,
                    RoomId = room.RoomId,
                    UserId = supervisor.Id,
                    IsActive = true
                };

                var complaint2 = new Complaint
                {
                    Title = "AC Not Working",
                    Description = "AC in room is broken",
                    DateTime = DateTime.Now.AddDays(-1),
                    RoomId = room.RoomId,
                    UserId = supervisor.Id,
                    IsActive = true
                };

                context.Complaints.AddRange(complaint1, complaint2);
                await context.SaveChangesAsync();
            }
        }
    }
}
