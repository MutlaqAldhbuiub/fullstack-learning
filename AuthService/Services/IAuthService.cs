using AuthService.Models;
using AuthService.Models.Dto;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationDto registrationDto);
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<string> setRoles(string userName, IList<string> roles);
    }

}