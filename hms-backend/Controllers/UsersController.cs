using Microsoft.AspNetCore.Mvc;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace hms_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserCountService _userCountService;

        public UsersController(IUserCountService userCountService)
        {
            _userCountService = userCountService;
        }

        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> GetUserCount()
        {
            var count = await _userCountService.GetUserCountAsync();
            return Ok(new { totalUsers = count });
        }
    }
}