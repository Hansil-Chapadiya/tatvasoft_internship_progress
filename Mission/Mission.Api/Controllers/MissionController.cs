using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission.Entities.context;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;

namespace Mission.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class MissionController : ControllerBase
    {
        private readonly MissionDbContext _context;
        public MissionController(MissionDbContext context)
        {
            _context = context;
        }

        [HttpPost("Add")]
        public IActionResult AddMission([FromBody] AddMissionDto dto)
        {
            var mission = new Mission.Entities.Entities.Mission
            {
                MissionTitle = dto.MissionTitle,
                MissionDescription = dto.MissionDescription,
                StartDate = dto.StartDate.ToUniversalTime(),
                EndDate = dto.EndDate.ToUniversalTime(),
                TotalSeats = dto.TotalSeats,
                MissionImage = dto.MissionImage,
                CountryId = dto.CountryId,
                CityId = dto.CityId,
                MissionThemeId = dto.MissionThemeId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false,
                MissionSkills = dto.SkillIds.Select(id => new MissionSkill
                {
                    SkillId = id
                }).ToList()
            };

            _context.Missions.Add(mission);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Mission added successfully"
            });
        }

        [HttpPut("Update")]
        public IActionResult UpdateMission([FromBody] UpdateMissionDto dto)
        {
            var mission = _context.Missions.FirstOrDefault(m => m.Id == dto.MissionId && !m.IsDeleted);
            if (mission == null)
                return NotFound(new
                {
                    success = false,
                    message = "Mission not found"
                });

            // Update basic fields
            mission.MissionTitle = dto.MissionTitle;
            mission.MissionDescription = dto.MissionDescription;
            mission.MissionThemeId = dto.MissionThemeId;
            mission.StartDate = dto.StartDate.ToUniversalTime();
            mission.EndDate = dto.EndDate.ToUniversalTime();
            mission.CityId = dto.CityId;
            mission.CountryId = dto.CountryId;
            mission.MissionImage = dto.MissionImage;
            mission.TotalSeats = dto.TotalSeats;
            mission.ModifiedDate = DateTime.UtcNow;

            // 🔄 Remove old skills
            var existingSkills = _context.MissionSkills.Where(ms => ms.MissionId == mission.Id).ToList();
            _context.MissionSkills.RemoveRange(existingSkills);

            // ➕ Add new skills
            foreach (var skillId in dto.SkillIds)
            {
                _context.MissionSkills.Add(new MissionSkill
                {
                    MissionId = mission.Id,
                    SkillId = skillId
                });
            }

            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "Mission updated successfully"
            });
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteMission(int id)
        {
            var mission = _context.Missions.FirstOrDefault(m => m.Id == id && !m.IsDeleted);

            if (mission == null)
                return NotFound(new
                {
                    success = false,
                    message = "Mission not found or already deleted."
                });

            // 🔁 Soft Delete
            mission.IsDeleted = true;
            mission.ModifiedDate = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Mission deleted successfully."
            });
        }


        [HttpGet("GetAll")]
        public IActionResult GetAllMissions()
        {
            var missions = _context.Missions
                .Where(m => !m.IsDeleted)
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.Theme)
                .Include(m => m.MissionSkills)
                    .ThenInclude(ms => ms.Skill)
                .Select(m => new GetMissionDto
                {
                    Id = m.Id,
                    MissionTitle = m.MissionTitle,
                    MissionDescription = m.MissionDescription,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    MissionImage = m.MissionImage,
                    TotalSeats = m.TotalSeats,
                    Country = m.Country.Name,
                    City = m.City.Name,
                    Theme = m.Theme.Title,
                    Skills = m.MissionSkills.Select(ms => ms.Skill.Name).ToList()
                })
                .ToList();

            return Ok(new
            {
                success = true,
                data = missions
            });
        }
    }
}
