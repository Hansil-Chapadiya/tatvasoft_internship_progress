using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;
using Mission.Entities.context;

namespace Mission.Api.Controllers
{

    public class MissionApplicationController : ControllerBase
    {
        private readonly MissionDbContext _context;

        public MissionApplicationController(MissionDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "user")]
        [HttpPost("Apply")]
        public IActionResult ApplyToMission([FromBody] MissionApplyDto dto)
        {
            var userId = dto.UserId;

            var mission = _context.Missions.FirstOrDefault(m => m.Id == dto.MissionId);
            if (mission == null)
                return NotFound(new { status = false, message = "Mission not found" });

            // Check if already applied
            bool alreadyApplied = _context.MissionApplications
                .Any(app => app.MissionId == dto.MissionId && app.UserId == userId);
            if (alreadyApplied)
                return BadRequest(new { success = false, message = "You have already applied for this mission." });

            // Check seat availability
            int approvedCount = _context.MissionApplications
                .Count(app => app.MissionId == dto.MissionId && app.Status == "Approved");

            if (approvedCount >= mission.TotalSeats)
                return BadRequest(new { success = false, message = "No seats available for this mission." });

            // Create application
            var application = new MissionApplication
            {
                MissionId = dto.MissionId,
                UserId = userId,
                Status = "Pending",
                ApplicationDate = DateTime.UtcNow
            };

            _context.MissionApplications.Add(application);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Application submitted successfully."
            });
        }

        [HttpPost("Applied")]
        public IActionResult CheckIfAlreadyApplied([FromBody] MissionApplyDto dto)
        {
            bool alreadyApplied = _context.MissionApplications
                .Any(app => app.MissionId == dto.MissionId && app.UserId == dto.UserId);

            return Ok(new
            {
                success = true,
                alreadyApplied = alreadyApplied
            });
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllApplications()
        {
            var applications = _context.MissionApplications
                .Include(ma => ma.Mission)
                .Include(ma => ma.User)
                .Select(app => new GetApplicationDto
                {
                    ApplicationId = app.Id,
                    MissionId = app.MissionId,
                    MissionTitle = app.Mission.MissionTitle,
                    UserId = app.UserId,
                    UserName = app.User.FirstName + " " + app.User.LastName,
                    ApplicationDate = app.ApplicationDate.Date,
                    Status = app.Status
                })
                .ToList();

            return Ok(new
            {
                success = true,
                data = applications
            });
        }

        [HttpPut("Approve/{applicationId}")]
        public IActionResult ApproveApplication(int applicationId)
        {
            var application = _context.MissionApplications
                .Include(a => a.Mission)
                .FirstOrDefault(a => a.Id == applicationId);

            if (application == null)
            {
                return NotFound(new { success = false, message = "Application not found." });
            }

            if (application.Status == "Approved")
            {
                return BadRequest(new { success = false, message = "Application is already approved." });
            }

            int approvedCount = _context.MissionApplications
                .Count(a => a.MissionId == application.MissionId && a.Status == "Approved");

            if (approvedCount >= application.Mission.TotalSeats)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No seats available for this mission."
                });
            }

            application.Status = "Approved";
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Application approved successfully.",
                status = application.Status
            });
        }

        [HttpPut("Reject/{applicationId}")]
        public IActionResult RejectApplication(int applicationId)
        {
            var application = _context.MissionApplications
                .FirstOrDefault(a => a.Id == applicationId);

            if (application == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Application not found."
                });
            }

            if (application.Status == "Rejected")
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Application is already rejected."
                });
            }

            application.Status = "Rejected";
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Application rejected successfully.",
                status = application.Status
            });
        }

        [HttpGet("MyApplications/{userId}")]
        public IActionResult GetUserApplications(int userId)
        {
            var applications = _context.MissionApplications
                .Include(app => app.Mission)
                .Where(app => app.UserId == userId)
                .Select(app => new
                {
                    missionTitle = app.Mission.MissionTitle,
                    applicationDate = app.ApplicationDate,
                    status = app.Status
                })
                .ToList();

            return Ok(new
            {
                success = true,
                data = applications
            });
        }

    }
}
