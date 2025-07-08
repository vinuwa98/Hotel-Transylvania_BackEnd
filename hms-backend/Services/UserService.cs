using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HmsBackend.Services
{
    public class UserService(UserManager<User> userManager, IUserRepository userRepository, IConfiguration configuration, AppDbContext appDbContext, IEmailService emailService) : IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository; // TODO: remove this
        private readonly IEmailService _emailService = emailService;

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
                userWithRoles.UserName = identityUser.UserName;
                userWithRoles.FirstName = identityUser.FirstName;
                userWithRoles.LastName = identityUser.LastName;

                return new DataTransferObject<LoginSuccessDto> { Message = "Login Successful", Data = userWithRoles };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("User login failed with: " + ex.Message);
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
                        FullName = $"{u.FirstName} {u.LastName}",
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Address = u.Address,
                        Role = u.Role,
                        ContactNumber = u.ContactNumber,
                        Status = u.IsActive ? "Active" : "Inactive",
                    })
                    .ToList();

                await SendResetPasswordEmailAsync(registerRequest.Email);

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
                        SupervisorId = u.UserCode.ToString(),
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

        public async Task<DataTransferObject<bool>> SendResetPasswordEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return new DataTransferObject<bool> { Data = false, Message = "User not found" };

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = $"http://localhost:5173/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";
                await _emailService.SendEmailAsync(email, "Reset Password", $"Go to reset: <a href=\"{link}\">Click here to reset your password</a>");

                return new DataTransferObject<bool> { Data = true, Message = "Reset email sent!" };
            }
            catch
            {
                throw new InvalidOperationException("Email sending unsuccessful!");
            }
        }

        public async Task<DataTransferObject<bool>> ResetPasswordAsync(ResetPasswordDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return new DataTransferObject<bool> { Message = "User not found", Data = false };

                var decodedToken = Uri.UnescapeDataString(model.Token);
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
                if (result.Succeeded)
                {
                    return new DataTransferObject<bool> { Message = "Password reset successful", Data = true };
                }

                throw new InvalidOperationException("Invalid token");
            }
            catch (Exception ex)
            {
                    throw new InvalidOperationException("Password reset operating failed with: " + ex.Message);
            }
        }

        public async Task<UserViewDto?> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            var userRole = await (
                from ur in _context.UserRoles
                join r in _context.Roles on ur.RoleId equals r.Id
                where ur.UserId == user.Id
                select r.Name
            ).FirstOrDefaultAsync();

            return new UserViewDto
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                FirstName = user.FirstName,     
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                ContactNumber = user.ContactNumber,
                Role = userRole ?? "Unknown",
                Status = user.IsActive ? "Active" : "Inactive"
            };
        }

        public async Task<DataTransferObject<List<User>>> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user == null)
                {
                    return new DataTransferObject<List<User>>
                    {
                        Message = "User not found",
                        Data = null
                    };
                }

                if (!string.IsNullOrEmpty(dto.Email))
                {
                    user.Email = dto.Email;
                    user.UserName = dto.Email; 
                }

                if (!string.IsNullOrEmpty(dto.ContactNumber))
                {
                    user.ContactNumber = dto.ContactNumber;
                }

                if (!string.IsNullOrEmpty(dto.FirstName))
                {
                    user.FirstName = dto.FirstName;
                }

                if (!string.IsNullOrEmpty(dto.LastName))
                {
                    user.LastName = dto.LastName;
                }

                if (!string.IsNullOrEmpty(dto.Address))
                {
                    user.Address = dto.Address;
                }

               

                if (dto.DOB.HasValue)
                {
                    user.DOB = dto.DOB.Value;
                }



                //Update password
                if (!string.IsNullOrEmpty(dto.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                    if (!passwordResult.Succeeded)
                    {
                        return new DataTransferObject<List<User>>
                        {
                            Message = "Password update failed",
                            Data = null
                        };
                    }
                }



                if (!string.IsNullOrEmpty(dto.Role))
                {
                    //Update roles if needed
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (!currentRoles.Contains(dto.Role))
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        if (!removeResult.Succeeded)
                        {
                            return new DataTransferObject<List<User>>
                            {
                                Message = "Failed to remove old roles",
                                Data = null
                            };
                        }

                        var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                        if (!addRoleResult.Succeeded)
                        {
                            return new DataTransferObject<List<User>>
                            {
                                Message = "Failed to add new role",
                                Data = null
                            };
                        }
                    }
                }

                //Final save
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new DataTransferObject<List<User>>
                    {
                        Message = "Failed to update user",
                        Data = null
                    };
                }

                // ✅ Fetch non-admin users
                var allUsers = await _userManager.Users.ToListAsync();
                var nonAdminUsers = new List<User>();

                foreach (var u in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    if (!roles.Contains("Admin"))
                    {
                        nonAdminUsers.Add(u);
                    }
                }

                return new DataTransferObject<List<User>>
                {
                    Message = "User updated successfully",
                    Data = nonAdminUsers
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataTransferObject<List<User>>
                {
                    Message = "An error occurred while updating the user",
                    Data = null
                };
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
            var roles = await _userManager.GetRolesAsync(identityUser);

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
                UserCode = Guid.NewGuid().ToString(),
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

        // View logged user name
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
