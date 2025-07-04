using HmsBackend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<DataTransferObject<List<User>>> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<bool> IsUserAlreadyExists(string email);
        Task<DataTransferObject<LoginSuccessDto>> LoginAsync(UserDto user);
        Task<DataTransferObject<List<UserViewDto>>> AddUserAsync(RegistrationDto registerRequest); // Corrected this line
        Task<List<UserViewDto>> GetAllUsersAsync();
        DataTransferObject<List<SupervisorInfoDto>> GetAllSupervisors();

        Task<bool> DeactivateUserAsync(string userId);

        Task<bool> ActivateUserAsync(string userId);
        Task<DataTransferObject<bool>> SendResetPasswordEmailAsync(string email);
        Task<DataTransferObject<bool>> ResetPasswordAsync(ResetPasswordDto model);
    }
}