using hms_backend.DTOs;
using HmsBackend.Dto;
using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest)
        {
            try
            {
                if (registerRequest == null)
                {
                    // You cannot return BadRequestResult here, since return type is IdentityResult
                    // Instead, return a failed IdentityResult with an error message
                    return IdentityResult.Failed(new IdentityError { Description = "RegisterRequest cannot be null" });
                }

                var result = await _userRepository.AddUserAsync(registerRequest);

                return result;  // Just return 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return IdentityResult.Failed(new IdentityError { Description = "An unexpected error occurred while adding the user." });

            }
        }


        /*

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
        */
        public async Task<IActionResult> LoginAsync(UserDto user)
        {
            try
            {
                var identityUser = await _userRepository.FindByEmailAsync(user.Email.Trim());



                if (identityUser != null)
                {
                    Console.WriteLine($"Found User: {identityUser.Email}");

                    var passwordCorrect = await _userRepository.CheckPasswordAsync(identityUser, user.Password);
                    Console.WriteLine($"Password Correct: {passwordCorrect}");

                    if (passwordCorrect)
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

                        Console.WriteLine($"Login Success: Token generated for {identityUser.Email}");
                        return new OkObjectResult(new { UserID = identityUser.Id, Token = token });
                    }
                }

                Console.WriteLine("Login Failed: Invalid email or password");
                return new NotFoundObjectResult("Invalid username or password");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new StatusCodeResult(500);
            }

        }



        public async Task<IActionResult> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                if (dto == null) return new BadRequestResult();

                var result = await _userRepository.UpdateUserAsync(dto);

                if (result.Succeeded)
                {
                    return new OkObjectResult("User updated successfully");
                }
                else
                {
                    return new BadRequestObjectResult(result.Errors);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new StatusCodeResult(500); // Internal Server Error
            }
        }

        // fetch all the users from the repository
        public async Task<List<UserViewDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        //public async Task<User?> FindUserByIdAsync(string userId)
        //{
        //    return await _userRepository.FindUserByIdAsync(userId);
        //}

        //public async Task<IdentityResult> UpdateUserStatusAsync(User user)
        //{
        //    return await _userRepository.UpdateUserStatusAsync(user);
        //}

    }
}
