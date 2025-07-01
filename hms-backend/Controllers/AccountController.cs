using hms_backend.DTOs;
using HmsBackend.Dto;
using HmsBackend.DTOs;
using HmsBackend.Models;
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
        public async Task<StandardResponseDto<List<User>>> AddUser(RegistrationDto registerRequest)
        {
            return await _userService.AddUserAsync(registerRequest);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDto user)
        {
            var result = await _userService.LoginAsync(user);
            return result;
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(updateUserDto);
            return result;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        //[Authorize(Policy = "AdminOnly")]
        //[HttpPut("toggle-user-status/{id}")]
        //public async Task<IActionResult> ToggleUserStatus(string id)
        //{
        //    var user = await _userService.FindUserByIdAsync(id);
        //    if (user == null)
        //        return NotFound("User not found");

        //    user.EmailConfirmed = !user.EmailConfirmed; // toggle Active/Inactive
        //    var result = await _userService.UpdateUserStatusAsync(user);

        //    if (result.Succeeded)
        //    {
        //        var status = user.EmailConfirmed ? "activated" : "deactivated";
        //        return Ok($"User has been {status}.");
        //    }

        //    return BadRequest("Failed to update user status.");
        //}


    }
}
