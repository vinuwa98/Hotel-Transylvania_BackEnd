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

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                // Optional: log the errors
                Console.WriteLine("User creation failed: " + string.Join(", ", errors));

                return BadRequest(new
                {
                    message = "User creation failed",
                    errors = errors
                });
            }

            return Ok(new
            {
                message = "User created successfully"
            });
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

    }
}
