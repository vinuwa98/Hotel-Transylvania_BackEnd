using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Repositories
{
    public class UserCountRepository : IUserCountRepository
    {
        //private readonly UserManager<User> _userManager;
        //private readonly AppDbContext _context;

        //public UserCountRepository(UserManager<User> userManager, AppDbContext context)
        //{
        //    _userManager = userManager;
        //    _context = context;
        //}

        //public async Task<int> GetTotalUserCountAsync()
        //{

        //    var users = await _userManager.Users.ToListAsync();

        //    var adminRoleId = await _context.Roles
        //        .Where(r => r.Name == "Admin")
        //        .Select(r => r.Id)
        //        .FirstOrDefaultAsync();

        //    var nonAdminCount = await _context.Users
        //        .Where(u => !_context.UserRoles
        //            .Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
        //        .CountAsync();



        //    return nonAdminCount;
        //}
    }
}
