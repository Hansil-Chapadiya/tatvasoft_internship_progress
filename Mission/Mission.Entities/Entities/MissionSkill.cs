using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.Entities
{
    public class MissionSkill
    {
        public int Id { get; set; } // This is the required primary key
        public int MissionId { get; set; }
        public Mission Mission { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }

}
