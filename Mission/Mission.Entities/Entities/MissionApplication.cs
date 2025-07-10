using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.Entities
{
    public class MissionApplication
    {
        public int Id { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }

}
