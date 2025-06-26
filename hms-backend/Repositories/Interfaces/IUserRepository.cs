using HmsBackend.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HmsBackend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest);
        Task<IdentityUser?> FindByEmailAsync(string username);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<IList<string>> GetRolesAsync(IdentityUser user);

        Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateDto);

    }
}

