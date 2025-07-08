using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Repositories
{
    public class UserRepository(UserManager<User> userManager, AppDbContext appDbContext) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _context = appDbContext;

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
                    SupervisorID = registerRequest.SupervisorID,
                    Role = registerRequest.Role
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
    }
}