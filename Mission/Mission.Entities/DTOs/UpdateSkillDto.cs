using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.DTOs
{
    public class UpdateSkillDto
    {
        public int Id { get; set; }               // Skill ID to update
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        //public int MissionId { get; set; }        // For mapping update (optional logic)
    }

}
