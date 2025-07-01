using HmsBackend.Models;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Services
{
    public class UserCountService : IUserCountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserCountService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<int> GetUserCountAsync()
        {
            var adminRoleId = await _context.Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var nonAdminCount = await _context.Users
                .Where(u => !_context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
                .CountAsync();



            return nonAdminCount;
        }
    }
}
