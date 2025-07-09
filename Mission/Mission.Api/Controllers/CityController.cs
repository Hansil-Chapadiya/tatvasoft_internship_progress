using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission.Entities.context;
using Mission.Entities.DTOs;
using Mission.Entities.Entities;

namespace Mission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly MissionDbContext _context;

        public CityController(MissionDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAllCities()
        {
            var skills = _context.Cities
                .Include(c => c.Country) // Include related Country data
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    CountryName = c.Country.Name, // Assuming Country has a Name
                })
                .ToList();

            return Ok(new { success = true, data = skills });
        }
    }
}
