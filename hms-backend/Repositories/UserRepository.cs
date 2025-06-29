using hms_backend.DTOs;
using HmsBackend.Dto;
using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace HmsBackend.Repositories
{
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest)
        {

            try
            {
                var normalizedEmail = registerRequest.Email.Trim().ToLower();

                var identityUser = new User
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
                    SupervisorID = registerRequest.SupervisorID
                };

                var result = await _userManager.CreateAsync(identityUser, registerRequest.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, registerRequest.Role);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return IdentityResult.Failed(new IdentityError { Description = "An error occurred while creating the user." });
            }

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

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<string>();
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = "User not found" });

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
                    return passwordResult;

                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(dto.Role))
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                        return removeResult;

                    var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!addRoleResult.Succeeded)
                        return addRoleResult;
                }

                return await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return IdentityResult.Failed(new IdentityError { Description = "An error occurred while updating the user." });
            }
        }

        public async Task<List<UserViewDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var nonAdminUsers = new List<UserViewDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Admin"))
                    {
                        nonAdminUsers.Add(new UserViewDto
                        {
                            FullName = (user.FirstName + " " + user.LastName).Trim(),
                            DOB = user.DOB,
                            Address = user.Address,
                            ContactNumber = user.ContactNumber,
                            Status = user.EmailConfirmed ? "Active" : "Inactive"
                        });
                    }  
                }

                return nonAdminUsers;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<UserViewDto>();
            }
        }


    }
}
