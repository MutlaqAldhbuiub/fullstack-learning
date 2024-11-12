using AuthService.Models;

namespace AuthService.Services
{

    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IList<string> roles);
    }
}