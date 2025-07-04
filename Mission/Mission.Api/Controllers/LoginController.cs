using Microsoft.AspNetCore.Mvc;
using Mission.Services.IServices;

namespace Mission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string EmailAddress, string Password)
        {
            var user = _loginService.login(EmailAddress, Password);
            if(user == null)
            {
                return NotFound("please check you email and password");
            }
            return Ok("login succesfully");
        }
    }
}
