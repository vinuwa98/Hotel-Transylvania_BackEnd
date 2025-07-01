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
    public class UserService(UserManager<User> userManager, IUserRepository userRepository, IConfiguration configuration) : IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository;

        private readonly UserManager<User> _userManager = userManager;

        public async Task<StandardResponseDto<List<User>>> AddUserAsync(RegistrationDto registerRequest)
        {
            try
            {
                if (registerRequest == null)
                {
                    return new StandardResponseDto<List<User>>(StatusCodes.Status400BadRequest, "Registration request cannot be null", null);
                }

                if (await _userManager.FindByEmailAsync(registerRequest.Email) != null)
                {
                    return new StandardResponseDto<List<User>>(StatusCodes.Status400BadRequest, "You can't create more than one user with the same username",null);
                }

                //var result = await _userRepository.AddUserAsync(registerRequest);
                var newUser = CreateNewUser(registerRequest);

                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, registerRequest.Role);
                }

                var users = _userManager.Users.ToList().Where(u => u.Role != "Admin").ToList();

                return new StandardResponseDto<List<User>>(StatusCodes.Status200OK, "User added successfully", users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new StandardResponseDto<List<User>>(StatusCodes.Status500InternalServerError, "User not added!", null);
            }
        }

        public async Task<StandardResponseDto<LoginSuccessDto>> LoginAsync(UserDto user)
        {
            try
            {
                var identityUser = await _userRepository.FindByEmailAsync(user.Email.Trim());

                if (identityUser != null)
                {
                    var passwordCorrect = await _userRepository.CheckPasswordAsync(identityUser, user.Password);

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

                        var result = new LoginSuccessDto { Token = token, UserId = identityUser.Id };

                        return new StandardResponseDto<LoginSuccessDto>(StatusCodes.Status200OK, "Login successful", result);
                    }
                }

                return new StandardResponseDto<LoginSuccessDto>(StatusCodes.Status500InternalServerError, $"Cannot find a user with email '{user.Email}'", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new StandardResponseDto<LoginSuccessDto>(StatusCodes.Status500InternalServerError, "Login failed", null);
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

        public async Task<List<UserViewDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        private User CreateNewUser(RegistrationDto registerRequest)
        {
            var normalizedEmail = registerRequest.Email.Trim().ToLower();

            return new User
            {
                UserName = registerRequest.Email,
                NormalizedUserName = registerRequest.Email.ToUpper(),
                Email = registerRequest.Email,
                NormalizedEmail = registerRequest.Email.ToUpper(),
                EmailConfirmed = true,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                DOB = registerRequest.DOB,
                Address = registerRequest.Address,
                ContactNumber = registerRequest.ContactNumber,
                SupervisorID = registerRequest.SupervisorID,
                Role = registerRequest.Role,
            };
        }
    }
}
