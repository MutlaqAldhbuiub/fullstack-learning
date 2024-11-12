using AuthService.Data;
using AuthService.Models;
using AuthService.Models.Dto;
using AuthService.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class AuthServicee : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthRepository _authRepository;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthServicee(
            RoleManager<IdentityRole> roleManager, IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _roleManager = roleManager;
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Register(RegistrationDto registrationDto)
        {
            User user = new()
            {
                UserName = registrationDto.Username,
                Email = registrationDto.Username,
                NormalizedUserName = registrationDto.Username.ToUpper(),
                name = registrationDto.Name,
                PhoneNumber = registrationDto.PhoneNumber,
            };

            try
            {
                var result = await _authRepository.createNewUser(user, registrationDto.Password);
                if (result.Succeeded)
                {
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception e)
            {
                return e.Message.FirstOrDefault().ToString();
            }
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            // TODO:: swap this by email.
            // var user = await _authRepository.getUserByUsername(loginDto.Username);
            var user = await _authRepository.getUserByUsername(loginDto.Username);
            var isValid = await _authRepository.checkUserPassword(user, loginDto.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto()
                {
                    Token = ""
                };
            }

            var roles = await _authRepository.GetUserRoles(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                ID = user.Id,
                Username = user.UserName,
                Name = user.name,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDto loginResponseDto = new()
            {
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> setRoles(string user, IList<string> roles)
        {
            var targetUser = await _authRepository.getUserByUsername(user);

            if (targetUser != null)
            {
                foreach (var role in roles)
                {
                    await _authRepository.setRole(targetUser, role);
                }

                return $"{user} Role Updated Successfully!";
            }
            return null;
        }
    }
}