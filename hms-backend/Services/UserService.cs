using HmsBackend.Dto;
using HmsBackend.DTOs;
using HmsBackend.Repositories.Interfaces;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HmsBackend.Services
{
    public class UserService(IUserRepository userRepository, IConfiguration configuration) : IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IActionResult> AddUserAsync(RegistrationDto registerRequest)
        {
            if (registerRequest == null) return new BadRequestResult();

            var result = await _userRepository.AddUserAsync(registerRequest);

            if (result.Succeeded)
            {
                return new OkObjectResult("Successfully added the user");
            }

            return new StatusCodeResult(500);
        }

        public async Task<IActionResult> LoginAsync(UserDto user)
        {
            var identityUser = await _userRepository.FindByEmailAsync(user.Email);

            if (identityUser != null && await _userRepository.CheckPasswordAsync(identityUser, user.Password))
            {
                var roles = await _userRepository.GetRolesAsync(identityUser);

                var claims = new List<Claim>
            {
                new Claim("UserId", identityUser.Id.ToString())
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
                var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

                return new OkObjectResult(new { UserID = identityUser.Id, Token = token });
            }

            return new NotFoundObjectResult("Invalid username or password");
        }
    }
}
