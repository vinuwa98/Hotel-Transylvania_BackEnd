using hms_backend.DTOs;
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
                if (identityUser == null)
                    throw new InvalidOperationException("Bad credentials!");

                var passwordCorrect = await CheckPasswordAsync(identityUser, user.Password);
                if (!passwordCorrect)
                    throw new InvalidOperationException("Bad credentials!");

                var userWithRoles = await AssignRoles(identityUser);
                return new DataTransferObject<LoginSuccessDto> { Message = "Login Successful", Data = userWithRoles };
            }
            catch
            {
                throw new InvalidOperationException("Unexpected exception occurred!");
            }
        }

        public async Task<DataTransferObject<List<UserViewDto>>> AddUserAsync(RegistrationDto registerRequest)
        {
            try
            {
                var newUser = CreateNewUser(registerRequest);
                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create user.");

                await _userManager.AddToRoleAsync(newUser, registerRequest.Role);

                var users = _userManager.Users
                    .Where(u => u.Role != "Admin")
                    .Select(u => new UserViewDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Address = u.Address,
                        Role = u.Role,
                        ContactNumber = u.ContactNumber,
                        Status = u.Status
                    })
                    .ToList();

                return new DataTransferObject<List<UserViewDto>> { Message = "User added successfully", Data = users };
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"User creation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while adding user.", ex);
            }
        }

        public DataTransferObject<List<SupervisorInfoDto>> GetAllSupervisors()
        {
            try
            {
                var supervisors = _userManager.Users
                    .Where(u => u.Role == "Supervisor")
                    .Select(u => new SupervisorInfoDto
                    {
                        SupervisorId = u.Id, // TODO: don't use db id here
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                    })
                    .ToList();

                return new DataTransferObject<List<SupervisorInfoDto>> { Message = null, Data = supervisors };
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while fetching all supervisors", ex);
            }
        }

        public async Task<DataTransferObject<string>> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                user.Email = dto.Email;
                user.UserName = dto.Email;
                user.ContactNumber = dto.ContactNumber;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Address = dto.Address;
                user.DOB = dto.DOB;

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!passwordResult.Succeeded)
                    throw new InvalidOperationException("Password reset failed.");

                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(dto.Role))
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                        throw new InvalidOperationException("Removing old roles failed.");

                    var addResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!addResult.Succeeded)
                        throw new InvalidOperationException("Assigning new role failed.");
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    throw new InvalidOperationException("User update failed.");

                return new DataTransferObject<string> { Message = "User updated successfully", Data = user.Id };
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"User update failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while updating user.", ex);
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
                        Status = user.EmailConfirmed ? "Active" : "Inactive"
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
                Status = "Active"
            };
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
    
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.EmailConfirmed = !user.EmailConfirmed;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
