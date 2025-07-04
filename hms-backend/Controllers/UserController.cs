﻿using HmsBackend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
