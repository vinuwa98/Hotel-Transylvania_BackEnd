using hms_backend.DTOs;
using HmsBackend.Dto;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<StandardResponseDto<List<User>>> AddUserAsync(RegistrationDto registerRequest);
        Task<StandardResponseDto<LoginSuccessDto>> LoginAsync(UserDto user);
        Task<IActionResult> UpdateUserAsync(UpdateUserDto updateUserDto);

        Task<List<UserViewDto>> GetAllUsersAsync();

        //Task<User?> FindUserByIdAsync(string userId);
        //Task<IdentityResult> UpdateUserStatusAsync(User user);


    }
}
