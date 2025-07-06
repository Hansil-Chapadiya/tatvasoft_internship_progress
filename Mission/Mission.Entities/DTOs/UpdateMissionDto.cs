using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.DTOs
{
    public class UpdateMissionDto
    {
        public int MissionId { get; set; }  // Mission ID
        public string MissionTitle { get; set; }
        public string MissionDescription { get; set; }
        public int MissionThemeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MissionImage { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int TotalSeats { get; set; }
        public List<int> SkillIds { get; set; }  // for updating skill mapping
    }

}
