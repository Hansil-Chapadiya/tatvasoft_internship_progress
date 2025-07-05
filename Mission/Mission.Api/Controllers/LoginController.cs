using Microsoft.AspNetCore.Mvc;
using Mission.Services.Helper;
using Mission.Services.IServices;

namespace Mission.Api.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class LoginController : Controller
    //{
    //    private readonly ILoginService _loginService;
    //    public LoginController(ILoginService loginService)
    //    {
    //        _loginService = loginService;
    //    }
    //    [HttpPost]
    //    [Route("Login")]
    //    public IActionResult Login(string EmailAddress, string Password)
    //    {
    //        var user = _loginService.login(EmailAddress, Password);
    //        if(user == null)
    //        {
    //            return NotFound("please check you email and password");
    //        }
    //        return Ok("login succesfully");
    //    }
    //}

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IConfiguration _config;

        public LoginController(ILoginService loginService, IConfiguration config)
        {
            _loginService = loginService;
            _config = config;
        }

        [HttpPost("Login")]
        public IActionResult Login(string EmailAddress, string Password)
        {
            var user = _loginService.login(EmailAddress, Password);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Please check your email and password."
                });
            }

            var secret = _config["JwtSettings:Secret"];
            var token_ = TokenHelper.GenerateJwtToken(user.Id.ToString(), user.EmailAddress, user.UserType, secret);

            return Ok(new
            {
                success = true,
                message = "Login successful",
                token = token_,
                data = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.EmailAddress,
                    user.UserType
                }
            });
        }
    }
}
