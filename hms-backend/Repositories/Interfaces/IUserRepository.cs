using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace HmsBackend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest);
        Task<User?> FindByEmailAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);

        // Remove or comment out this line as UserService directly handles it
        // Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateDto); 

        //Task<List<UserViewDto>> GetAllUsersAsync(); // These were already commented, keep them that way if not used
        //Task<bool> DeactivateUserAsync(string userId); // These were already commented, keep them that way if not used
    }
}