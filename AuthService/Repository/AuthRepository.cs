using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<User> _userManager;

        public AuthRepository(AuthDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IdentityResult> createNewUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<User> getUserByUsername(string username)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            return result;
        }

        public async Task<bool> checkUserPassword(User user, string password)
        {
            var result = await _userManager.CheckPasswordAsync(user, password);

            return result;
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task setRole(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);

        }
    }
}