using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Repositories
{
    public class UserCountRepository : IUserCountRepository
    {
        private readonly UserManager<User> _userManager;

        public UserCountRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            int nonAdminCount = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    nonAdminCount++;
                }
            }

            return nonAdminCount;
        }
    }
}
