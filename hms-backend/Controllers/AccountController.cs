using HmsBackend.Dto;
using HmsBackend.DTOs;
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
    public class AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration) : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        [Authorize(Policy = "AdminOnly")]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDto registerRequest)
        {
            try
            {
                if (registerRequest == null) return BadRequest();

                var identityUser = new IdentityUser
                {
                    UserName = registerRequest.User.UserName,
                    Email = registerRequest.User.Email
                };

                var result = await _userManager.CreateAsync(identityUser, registerRequest.User.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, registerRequest.Role);
                    return NoContent();
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDto user)
        {
            try
            {
                var _user = await _userManager.FindByNameAsync(user.UserName);

                if (_user != null && await _userManager.CheckPasswordAsync(_user, user.Password))
                {
                    var roles = await _userManager.GetRolesAsync(_user);

                    var claims = new List<Claim>
                    {
                        new Claim("UserId", _user.Id.ToString())
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Audience"],
                        Expires = DateTime.UtcNow.AddMonths(1),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                            SecurityAlgorithms.HmacSha256Signature
                        ),
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);

                    return Ok(new { UserID = _user.Id, Token = token });
                }
                else
                {
                    return NotFound("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
    }
}
