using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HmsBackend.Services
{
    public class UserService(UserManager<User> userManager, IUserRepository userRepository, IConfiguration configuration,AppDbContext appDbContext) : IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository; // TODO: remove this

        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _context = appDbContext;
      

        public async Task<DataTransferObject<LoginSuccessDto>> LoginAsync(UserDto user)
        {
            try
            {
                var identityUser = await FindByEmailAsync(user.Email.Trim());

                if (identityUser != null)
                {
                    var passwordCorrect = await CheckPasswordAsync(identityUser, user.Password);

                    if (passwordCorrect)
                    {
                        var userWithRoles = await AssignRoles(identityUser);

                        return new DataTransferObject<LoginSuccessDto> { Message = "Login Successful", Data = userWithRoles };
                    }
                }

                return new DataTransferObject<LoginSuccessDto> { Message = $"Cannot find a user with email '{user.Email}'", Data = null };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataTransferObject<LoginSuccessDto> { Message = "Login failed", Data = null };
            }
        }

        public async Task<DataTransferObject<List<User>>> AddUserAsync(RegistrationDto registerRequest)
        {
            try
            {
                var newUser = CreateNewUser(registerRequest);

                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, registerRequest.Role);
                }

                var users = _userManager.Users.ToList().Where(u => u.Role != "Admin").ToList();

                return new DataTransferObject<List<User>> { Message = "User added successfully", Data = users };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new DataTransferObject<List<User>> { Message = "User addition failed", Data = null };
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
           
            try
            {
                // Get the Admin Role ID
                var adminRoleId = await _context.Roles
                    .Where(r => r.Name == "Admin")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                // Fetch non-admin users using joins
                var users = await (
                    from user in _context.Users
                    join userRole in _context.UserRoles on user.Id equals userRole.UserId
                    join role in _context.Roles on userRole.RoleId equals role.Id
                    where userRole.RoleId != adminRoleId
                    select new UserViewDto
                    {
                        Id = user.Id,
                        FullName = (user.FirstName + " " + user.LastName).Trim(),
                        Role = role.Name,
                        Address = user.Address,
                        ContactNumber = user.ContactNumber,
                        Status = user.IsActive ? "Active" : "Inactive"
                    }).ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching users: " + ex.Message);
                return new List<UserViewDto>();
            }
        }

        public async Task<bool> IsUserAlreadyExists(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            return existingUser != null;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            try
            {
                if (email == string.Empty || email == null)
                {
                    throw new Exception("Cannot find users with invalid email");
                }

                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            try
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private async Task<LoginSuccessDto> AssignRoles(User identityUser)
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

            return result;
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

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Where (u => u.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null) return false;

                user.IsActive = false;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null) return false;


                user.IsActive = true;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //public async Task<List<UserViewDto>> GetLoggedUserName()
        //{


        //}

        public async Task<string?> GetLoggedUserFullNameAsync(ClaimsPrincipal userClaims)
        {
            try
            {
                var userId = userClaims.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userId))
                    return null;

                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => (u.FirstName + " " + u.LastName).Trim())
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching logged-in user name: " + ex.Message);
                return null;
            }
        }


    }
}
