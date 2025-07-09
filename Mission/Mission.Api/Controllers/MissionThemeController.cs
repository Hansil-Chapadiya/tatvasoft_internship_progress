using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.context;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;

namespace Mission.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MissionThemeController : ControllerBase
    {
        private readonly MissionDbContext _context;

        public MissionThemeController(MissionDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAllThemes(bool onlyActive = false)
        {
            var themes = onlyActive
                ? _context.MissionThemes.Where(t => t.IsActive).ToList()
                : _context.MissionThemes.ToList();

            return Ok(new { success = true, data = themes });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        public IActionResult AddTheme([FromBody] AddMissionThemeDto dto)
        {
            if (_context.MissionThemes.Any(t => t.Title.ToLower() == dto.Title.ToLower()))
            {
                return BadRequest("Theme with the same title already exists.");
            }

            var theme = new MissionTheme
            {
                Title = dto.Title,
                IsActive = dto.IsActive
            };

            _context.MissionThemes.Add(theme);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Mission Theme added." });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("Update")]
        public IActionResult UpdateTheme([FromBody] UpdateMissionThemeDto dto)
        {
            var theme = _context.MissionThemes.FirstOrDefault(t => t.Id == dto.Id);
            if (theme == null)
                return NotFound("Theme not found.");

            theme.Title = dto.Title;
            theme.IsActive = dto.IsActive;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Mission Theme updated." });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteTheme(int id)
        {
            var theme = _context.MissionThemes.FirstOrDefault(t => t.Id == id);
            if (theme == null)
                return NotFound();

            theme.IsActive = false;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Theme deactivated" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("ToggleStatus/{id}")]
        public IActionResult ToggleThemeStatus(int id)
        {
            var theme = _context.MissionThemes.FirstOrDefault(t => t.Id == id);
            if (theme == null)
                return NotFound();

            theme.IsActive = !theme.IsActive;
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = $"Theme status set to {(theme.IsActive ? "Active" : "Inactive")}"
            });
        }
    }

}
