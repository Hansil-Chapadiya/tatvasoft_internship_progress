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

        [Authorize(Roles = "admin")]
        [HttpPut("Update")]
        public IActionResult UpdateUser([FromBody] UpdateUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == dto.Id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User not found."
                });
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.UserImage = dto.UserImage;
            user.ModifiedDate = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = PasswordHasher.HashPassword(dto.Password);
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "User updated successfully.",
                data = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.EmailAddress,
                    user.PhoneNumber,
                    user.UserImage,
                    user.ModifiedDate
                }
            });
        }

        //[Authorize(Roles = "admin")]
        [HttpGet("All")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.EmailAddress,
                    u.PhoneNumber,
                    u.UserType,
                    u.UserImage,
                    u.CreatedDate,
                    u.ModifiedDate
                })
                .ToList();

            return Ok(new
            {
                success = true,
                message = "User list fetched successfully.",
                data = users
            });
        }

        //[Authorize(Roles = "admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User not found or already deleted."
                });
            }

            user.IsDeleted = true;
            user.ModifiedDate = DateTime.UtcNow;

            _context.Users.Update(user);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = $"User with ID {id} deleted successfully."
            });
        }

    }
}
