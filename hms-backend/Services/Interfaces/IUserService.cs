using HmsBackend.Dto;
using HmsBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> AddUserAsync(RegistrationDto registerRequest);
        Task<IActionResult> LoginAsync(UserDto user);
    }
}
