using HmsBackend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
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
            if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Email and password are required.");

            try
            {
                var result = await _userService.LoginAsync(user);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [Route("add-user")]
        [HttpPost]
        public async Task<IActionResult> AddUser(RegistrationDto registerRequest)
        {
            if (registerRequest == null)
                return BadRequest("Invalid registration details!");

            var isUserAlreadyExists = await _userService.IsUserAlreadyExists(registerRequest.Email);
            if (isUserAlreadyExists)
                return Conflict("Duplicate registration detected!");

            try
            {
                var allUsers = await _userService.AddUserAsync(registerRequest);
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [Route("get-supervisors")]
        [HttpGet]
        public IActionResult GetAllSupervisors()
        {
            try
            {
                var allUsers = _userService.GetAllSupervisors();
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            var result = await _userService.SendResetPasswordEmailAsync(forgotPassword.Email);
            if (result.Data)
            {
                return Ok(result.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
        }

        [AllowAnonymous]
        [Route("reset-password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var result = await _userService.ResetPasswordAsync(model);
            if (result.Data)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id); // You must have this in your service
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _userService.UpdateUserAsync(dto);
                if (result.Data == null)
                    return NotFound(new { message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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