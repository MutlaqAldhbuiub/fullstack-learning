using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models.Dto;
using AuthService.Services;
using System.Threading.Tasks;
using System.Net.Http;

namespace AuthService.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;
        protected ResponseDto _response;

        public AuthController(HttpClient httpClient, IAuthService authService)
        {
            _authService = authService;
            _response = new();
            _httpClient = httpClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            var errorMessage = await _authService.Register(registrationDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return Unauthorized(errorMessage);
            }

            return Ok(_response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.Login(loginDto);
            if (loginResult.Token == "")
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect.";
                return Unauthorized(_response);
            }


            // Update the network status to "true" after successful login
            var networkId = loginDto.NetworkId;
            var networkUpdateRequest = new
            {
                Status = "true"
            };

            // Construct the URL for the PUT request
            var networkServiceUrl = $"http://localhost:5278/api/Networks/{networkId}?status=true";

            // Send the PUT request
            var response = await _httpClient.PutAsync(networkServiceUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to update network status.";
                return StatusCode((int)response.StatusCode, _response);
            }


            _response.Result = loginResult;
            return Ok(_response);
        }


        [HttpPost("setRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> setRoles([FromBody] RolesDto rolesDto)
        {
            var result = await _authService.setRoles(rolesDto.username, rolesDto.roles);
            if (result != null)
            {
                _response.IsSuccess = true;
                _response.Message = result;
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Message = result;
            return BadRequest(_response);
        }

    }
}