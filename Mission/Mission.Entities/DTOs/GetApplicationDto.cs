using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.DTOs
{
    public class GetApplicationDto
    {
        public int ApplicationId { get; set; }
        public int MissionId { get; set; }
        public string MissionTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
    }

}
