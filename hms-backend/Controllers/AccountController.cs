using HmsBackend.Dto;
using HmsBackend.DTOs;
using HmsBackend.Services;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HmsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IUserService userService, IConfiguration configuration) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;

        [Authorize(Policy = "AdminOnly")]
        [Route("add-user")]
        [HttpPost]
        public async Task<IActionResult> AddUser(RegistrationDto registerRequest)
        {
            var result = await _userService.AddUserAsync(registerRequest);
            return result;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDto user)
        {
            var result = await _userService.LoginAsync(user);
            return result;
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        [Route("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, UpdateUserDto updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);
            return result;
        }
    }
}
