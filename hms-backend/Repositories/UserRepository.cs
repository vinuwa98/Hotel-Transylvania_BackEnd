using HmsBackend.DTOs;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HmsBackend.Repositories
{
    public class UserRepository(UserManager<IdentityUser> userManager) : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(identityUser, registerRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, registerRequest.Role);
            }

            return result;
        }

        public Task<IdentityUser?> FindByEmailAsync(string username)
        {
            return _userManager.FindByEmailAsync(username);
        }

        public Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            // Update basic IdentityUser fields
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.PhoneNumber = dto.ContactNumber;

            // Update Password (generate reset token first)
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
            if (!passwordResult.Succeeded)
                return passwordResult;

            // Update roles if needed
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(dto.Role))
            {
                // Remove all current roles and add the new one
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return removeResult;

                var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                if (!addRoleResult.Succeeded)
                    return addRoleResult;
            }

            // Save changes
            return await _userManager.UpdateAsync(user);
        }


    }
}
