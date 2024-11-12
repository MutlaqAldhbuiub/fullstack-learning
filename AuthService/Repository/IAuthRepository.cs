using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Repository
{

    public interface IAuthRepository
    {
        Task<IdentityResult> createNewUser(User user, string password);
        Task<User> getUserByUsername(string username);
        Task<bool> checkUserPassword(User user, string password);
        Task<IList<string>> GetUserRoles(User user);
        Task setRole(User user, string role);
    }
}