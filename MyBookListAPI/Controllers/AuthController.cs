using Microsoft.AspNetCore.Mvc;

namespace MyBookListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> AuthStatus()
        {
            return Ok("auth status");
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
        public ActionResult<string> Signup()
        {
            return Ok("signup");
        }
    }
}
