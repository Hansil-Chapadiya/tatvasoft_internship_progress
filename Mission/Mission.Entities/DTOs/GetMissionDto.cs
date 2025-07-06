using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.DTOs
{
    public class GetMissionDto
    {
        public int Id { get; set; }
        public string MissionTitle { get; set; }
        public string MissionDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MissionImage { get; set; }
        public int TotalSeats { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Theme { get; set; }
        public List<string> Skills { get; set; }
    }

}
