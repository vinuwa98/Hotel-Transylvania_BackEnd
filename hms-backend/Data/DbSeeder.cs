using HmsBackend;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedDatabaseAsync(AppDbContext context, UserManager<User> userManager)
    {
        if (!context.Users.Any())
        {
            var admin = new User { Id = Guid.NewGuid().ToString(), UserCode = "U001", UserName = "admin", Email = "admin@example.com", Role = "Admin", IsActive = true };
            var manager = new User { Id = Guid.NewGuid().ToString(), UserCode = "U002", UserName = "manager", Email = "manager@example.com", Role = "Manager", IsActive = true };
            var supervisor = new User { Id = Guid.NewGuid().ToString(), UserCode = "U003", UserName = "supervisor", Email = "supervisor@example.com", Role = "Supervisor", IsActive = true };
            var cleaner = new User { Id = Guid.NewGuid().ToString(), UserCode = "U004", UserName = "cleaner", Email = "cleaner@example.com", Role = "Cleaner", IsActive = true };

            await userManager.CreateAsync(admin, "Pass@123");
            await userManager.CreateAsync(manager, "Pass@123");
            await userManager.CreateAsync(supervisor, "Pass@123");
            await userManager.CreateAsync(cleaner, "Pass@123");
        }

        var users = userManager.Users.ToList();
        var supervisorUser = users.FirstOrDefault(u => u.Role == "Supervisor");
        var managerUser = users.FirstOrDefault(u => u.Role == "MaintenanceManager");
        var cleanerUser = users.FirstOrDefault(u => u.Role == "Cleaner");
        var adminUser = users.FirstOrDefault(u => u.Role == "Admin");

        if (!context.Rooms.Any())
        {
            var rooms = new List<Room>
            {
                new() { RoomId = Guid.NewGuid().ToString(), RoomNumber = "R101", RoomType = "Single" },
                new() { RoomId = Guid.NewGuid().ToString(), RoomNumber = "R102", RoomType = "Double" },
                new() { RoomId = Guid.NewGuid().ToString(), RoomNumber = "R103", RoomType = "Twin" },
                new() { RoomId = Guid.NewGuid().ToString(), RoomNumber = "R104", RoomType = "Suite" },
                new() { RoomId = Guid.NewGuid().ToString(), RoomNumber = "R105", RoomType = "Deluxe" },
                new() { RoomId = Guid.NewGuid().ToString(), RoomNumber = "R106", RoomType = "Family" },
            };
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();

            var roomStatuses = rooms.Select((r, index) => new RoomStatus { Status = new[] { "Occupied", "Reserved", "Available", "Maintenance" }[index % 4], Room = r }).ToList();
            context.RoomStatus.AddRange(roomStatuses);
            await context.SaveChangesAsync();
        }

        var firstRoom = context.Rooms.First();
        if (!context.Complaints.Any() && supervisorUser != null)
        {
            var complaint1 = new Complaint
            {
                Id = Guid.NewGuid().ToString(),
                ComplaintNumber = "C001",
                Title = "Leaking Pipe",
                Description = "Pipe in room leaking",
                DateTime = DateTime.Now,
                RoomId = firstRoom.RoomId,
                UserId = supervisorUser.Id,
                IsActive = true
            };

            context.Complaints.Add(complaint1);
            await context.SaveChangesAsync();

            context.ComplaintCleaners.Add(new ComplaintCleaner
            {
                Id = Guid.NewGuid().ToString(),
                ComplaintId = complaint1.Id,
                CleanerId = cleanerUser.Id
            });
            await context.SaveChangesAsync();
        }

        var complaint = context.Complaints.FirstOrDefault();
        if (!context.Job.Any() && complaint != null && managerUser != null && adminUser != null)
        {
            var job = new Job
            {
                Id = Guid.NewGuid().ToString(),
                JobNumber = "J001",
                Name = "Fix Pipe",
                Description = "Fix the broken pipe",
                Status = "Open",
                Priority = "High",
                CreatedUserId = adminUser.Id,
                AssignedManagerUserId = managerUser.Id,
                ComplaintId = complaint.Id,
                IsDeleted = false
            };

            context.Job.Add(job);
            await context.SaveChangesAsync();

            context.JobUsers.Add(new JobUser
            {
                JobId = job.Id,
                UserId = cleanerUser.Id
            });
            await context.SaveChangesAsync();
        }
    }

}
