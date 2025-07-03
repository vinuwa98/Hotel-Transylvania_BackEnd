using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<DataTransferObject<List<UserViewDto>>> AddUserAsync(RegistrationDto registerRequest);
        Task<bool> IsUserAlreadyExists(string email);
        Task<DataTransferObject<LoginSuccessDto>> LoginAsync(UserDto user);
        Task<DataTransferObject<string>> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<List<UserViewDto>> GetAllUsersAsync();
        DataTransferObject<List<UserViewDto>> GetAllSupervisors();

        Task<bool> DeactivateUserAsync(string userId);
    }
}
