using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.context;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly MissionDbContext _context;

        public SkillController(MissionDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAllSkills(bool onlyActive = false)
        {
            var skills = onlyActive
                ? _context.Skills.Where(s => s.IsActive).ToList()
                : _context.Skills.ToList();

            return Ok(new { success = true, data = skills });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        public IActionResult AddSkill([FromBody] AddSkillDto dto)
        {
            if (_context.Skills.Any(s => s.Name.ToLower() == dto.Name.ToLower()))
            {
                return BadRequest("Skill with the same name already exists.");
            }

            var skill = new Skill
            {
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            _context.Skills.Add(skill);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Skill added successfully." });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("Update")]
        public IActionResult UpdateSkill([FromBody] UpdateSkillDto dto)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == dto.Id);
            if (skill == null)
                return NotFound("Skill not found.");

            skill.Name = dto.Name;
            skill.IsActive = dto.IsActive;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Skill updated successfully." });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteSkill(int id)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == id);
            if (skill == null)
                return NotFound();

            skill.IsActive = false;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Skill deactivated successfully." });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("ToggleStatus/{id}")]
        public IActionResult ToggleSkillStatus(int id)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == id);
            if (skill == null)
                return NotFound();

            skill.IsActive = !skill.IsActive;
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = $"Skill status set to {(skill.IsActive ? "Active" : "Inactive")}."
            });
        }
    }
}
