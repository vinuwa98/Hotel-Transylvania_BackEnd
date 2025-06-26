using HmsBackend.DTOs;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HmsBackend.Repositories
{
    public class UserRepository(UserManager<IdentityUser> userManager) : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(identityUser, registerRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, registerRequest.Role);
            }

            return result;
        }

        public Task<IdentityUser?> FindByEmailAsync(string username)
        {
            return _userManager.FindByEmailAsync(username);
        }

        public Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            return _userManager.GetRolesAsync(user);
        }
    }
}
