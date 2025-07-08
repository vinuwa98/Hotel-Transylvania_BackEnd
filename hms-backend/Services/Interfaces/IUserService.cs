using hms_backend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HmsBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<DataTransferObject<List<User>>> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<bool> IsUserAlreadyExists(string email);
        Task<DataTransferObject<LoginSuccessDto>> LoginAsync(UserDto user);
        Task<DataTransferObject<List<UserViewDto>>> AddUserAsync(RegistrationDto registerRequest); // Corrected this line
        Task<List<UserViewDto>> GetAllUsersAsync();
        Task<UserViewDto?> GetUserByIdAsync(string userId);
        DataTransferObject<List<SupervisorInfoDto>> GetAllSupervisors();

        Task<bool> DeactivateUserAsync(string userId);

        Task<bool> ActivateUserAsync(string userId);

        Task<string?> GetLoggedUserFullNameAsync(ClaimsPrincipal userClaims);
    }
}