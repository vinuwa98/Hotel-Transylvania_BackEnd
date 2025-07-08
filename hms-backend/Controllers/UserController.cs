using HmsBackend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HmsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService, IConfiguration configuration, IUserCountService userCountService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IUserCountService _userCountService = userCountService;

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserDto user)
        {
            if (user == null)
            {
                return BadRequest("Invalid login details!");
            }

            var result = await _userService.LoginAsync(user);

            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [Route("add-user")]
        [HttpPost]
        public async Task<IActionResult> AddUser(RegistrationDto registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("Invalid registration details!");
            }

            var isUserAlreadyExists = await _userService.IsUserAlreadyExists(registerRequest.Email);

            if (isUserAlreadyExists)
            {
                return BadRequest("Duplicate registration detected!");
            }

            var allUsers = await _userService.AddUserAsync(registerRequest);

            return Ok(allUsers);
        }

        [Authorize(Policy = "AdminOnly")]
        [Route("update-user")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(updateUserDto);
            return result;
        }

        [Authorize(Policy = "AdminOnly")]
        [Route("get-users")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Policy = "AdminOnly")]
        [Route("get-count")]
        [HttpGet]
        public async Task<IActionResult> GetUserCount()
        {
            var count = await _userCountService.GetUserCountAsync();
            return Ok(new { totalUsers = count });
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPut("deactivate-user")]
        public async Task<IActionResult> DeactivateUser([FromBody] string userId)
        {
            var success = await _userService.DeactivateUserAsync(userId);
            if (!success)
                return NotFound(new { message = "User not found or could not be deactivated" });

            return Ok(new { message = "User deactivated successfully" });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("activate-user")]
        public async Task<IActionResult> ActivateUser([FromBody] string userId)
        {
            var success = await _userService.ActivateUserAsync(userId);
            if (!success)
                return NotFound(new { message = "User not found or already active" });

            return Ok(new { message = "User activated successfully" });
        }

        [Authorize]
        [HttpGet("get-logged-user")]
        public async Task<IActionResult> GetLoggedUser()
        {
            var fullName = await _userService.GetLoggedUserFullNameAsync(User);

            if (fullName == null)
                return NotFound(new { message = "User not found or not authorized" });

            return Ok(new { fullName });
        }
    }
}
