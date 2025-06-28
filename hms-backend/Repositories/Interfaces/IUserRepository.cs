using hms_backend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest);
        Task<User?> FindByEmailAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);

        Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateDto);


        Task<List<ViewUserDto>> GetAllUsersAsync();

        Task<ViewUserDto?> ViewUserAsync(string userId);

    }
}


