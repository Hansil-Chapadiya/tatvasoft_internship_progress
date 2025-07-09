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
    public class CountryController : ControllerBase
    {
        private readonly MissionDbContext _context;
        public CountryController(MissionDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAllCountries()
        {
            var countries = _context.Countries.ToList();
            return Ok(new { success = true, data = countries });
        }
    } 
}
