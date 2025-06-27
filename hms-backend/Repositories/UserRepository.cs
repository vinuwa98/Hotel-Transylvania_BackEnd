using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace HmsBackend.Repositories
{
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest)
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

        public Task<User?> FindByEmailAsync(string username)
        {
            return _userManager.FindByEmailAsync(username);
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.PhoneNumber = dto.ContactNumber;
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



    }
}
