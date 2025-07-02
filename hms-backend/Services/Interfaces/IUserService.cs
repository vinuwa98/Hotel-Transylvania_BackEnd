using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<DataTransferObject<List<User>>> AddUserAsync(RegistrationDto registerRequest);
        Task<bool> IsUserAlreadyExists(string email);
        Task<DataTransferObject<LoginSuccessDto>> LoginAsync(UserDto user);
        Task<IActionResult> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<List<UserViewDto>> GetAllUsersAsync();

        Task<bool> DeactivateUserAsync(string userId);
       


    }
}
