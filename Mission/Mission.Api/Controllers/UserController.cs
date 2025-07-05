using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.context;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;
using Mission.Services.Helper;

namespace Mission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MissionDbContext _context;

        public UserController(MissionDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        public IActionResult AddUser([FromBody] AddUserDto userDto)
        {
            if (_context.Users.Any(u => u.EmailAddress == userDto.EmailAddress))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "User with this email already exists."
                });
            }

            var newUser = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                EmailAddress = userDto.EmailAddress,
                Password = PasswordHasher.HashPassword(userDto.Password),
                PhoneNumber = userDto.PhoneNumber,
                UserType = userDto.UserType,
                UserImage = userDto.UserImage,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "User added successfully."
            });
        }
    }
}
