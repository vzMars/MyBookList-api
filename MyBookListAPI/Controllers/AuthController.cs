using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;
using System.Security.Claims;

namespace MyBookListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<AuthResponse> AuthStatus()
        {
            var response = new AuthResponse
            {
                Success = true,
                User = new User
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                    Email = User.FindFirstValue(ClaimTypes.Email)!,
                    Username = User.FindFirstValue(ClaimTypes.Name)!,
                }
            };

            return Ok(response);
        }

        [HttpPost("Login")]
        public ActionResult<string> Login()
        {
            return Ok("login");
        }

        [HttpGet("Logout")]
        public ActionResult<string> Logout()
        {
            return Ok("logout");
        }

        [HttpPost("Signup")]
        public async Task<ActionResult<AuthResponse>> Signup(SignupRequest request)
        {
            var response = await _authRepository.Signup(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
