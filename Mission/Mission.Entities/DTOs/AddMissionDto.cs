using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.DTOs
{
    public class AddMissionDto
    {
        public string MissionTitle { get; set; } = string.Empty;
        public string MissionDescription { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSeats { get; set; }
        public string MissionImage { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int MissionThemeId { get; set; }
        public List<int> SkillIds { get; set; } = new(); // Scrollable List
    }

}
