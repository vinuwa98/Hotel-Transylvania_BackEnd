using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HmsBackend.Repositories
{
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<IdentityResult> AddUserAsync(RegistrationDto registerRequest)
        {
            try
            {
                var identityUser = new User
                {
                    UserName = registerRequest.Email,
                    Email = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    DOB = registerRequest.DOB,
                    Address = registerRequest.Address,
                    PhoneNumber = registerRequest.ContactNumber
                };

                var result = await _userManager.CreateAsync(identityUser, registerRequest.Password);

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(identityUser, registerRequest.Role);

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
    }
}
