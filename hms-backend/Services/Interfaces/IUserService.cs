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
        Task<bool> DeactivateUserAsync(string userId);
    }
}