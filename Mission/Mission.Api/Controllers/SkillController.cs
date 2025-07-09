using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.context;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;
using Mission.Services.Helper;

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
            // Optional: Check if skill already exists by name
            if (_context.Skills.Any(s => s.Name.ToLower() == dto.Name.ToLower()))
            {
                return BadRequest("Skill with the same name already exists.");
            }

            // Step 1: Add new Skill (auto-generated Id)
            var skill = new Skill
            {
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            _context.Skills.Add(skill);
            _context.SaveChanges(); // now skill.Id is generated

            // Step 2: Map this skill to Mission using MissionSkill
            var missionExists = _context.Missions.Any(m => m.Id == dto.MissionId);
            if (!missionExists)
            {
                return BadRequest("Invalid MissionId provided.");
            }

            var missionSkill = new MissionSkill
            {
                MissionId = dto.MissionId,
                SkillId = skill.Id // use generated Id here
            };

            _context.MissionSkills.Add(missionSkill);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Skill added and mapped to mission." });
        }



        [Authorize(Roles = "admin")]
        [HttpPut("Update")]
        public IActionResult UpdateSkill([FromBody] UpdateSkillDto dto)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == dto.Id);
            if (skill == null)
                return NotFound("Skill not found.");

            // 1. Update skill properties
            skill.Name = dto.Name;
            skill.IsActive = dto.IsActive;
            _context.SaveChanges();

            // 2. Update mapping in MissionSkills table if needed
            var existingMapping = _context.MissionSkills.FirstOrDefault(ms => ms.SkillId == dto.Id);

            if (existingMapping != null && existingMapping.MissionId != dto.MissionId)
            {
                // Update mission mapping
                existingMapping.MissionId = dto.MissionId;
                _context.SaveChanges();
            }
            else if (existingMapping == null)
            {
                // Create new mapping if doesn't exist
                var newMapping = new MissionSkill
                {
                    MissionId = dto.MissionId,
                    SkillId = dto.Id
                };
                _context.MissionSkills.Add(newMapping);
                _context.SaveChanges();
            }

            return Ok(new { success = true, message = "Skill updated successfully." });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteSkill(int id)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == id);
            if (skill == null) return NotFound();

            skill.IsActive = false;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Skill deactivated" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("ToggleStatus/{id}")]
        public IActionResult ToggleSkillStatus(int id)
        {
            var skill = _context.Skills.FirstOrDefault(s => s.Id == id);
            if (skill == null) return NotFound();

            skill.IsActive = !skill.IsActive;
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = $"Skill status set to {(skill.IsActive ? "Active" : "Inactive")}"
            });
        }


        // ----------------- CRUD API below ------------------
    }
}
