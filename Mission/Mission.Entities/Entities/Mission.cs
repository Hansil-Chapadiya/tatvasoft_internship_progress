using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.Entities
{
    [Table("Mission")]
    public class Mission : Base
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string MissionTitle { get; set; } = string.Empty;

        [Column("description")]
        public string MissionDescription { get; set; } = string.Empty;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("total_seats")]
        public int TotalSeats { get; set; }

        [Column("image")]
        public string MissionImage { get; set; } = string.Empty;

        // Foreign Keys
        [ForeignKey("Country")]
        [Column("country_id")]
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        [ForeignKey("City")]
        [Column("city_id")]
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        [ForeignKey("Theme")]
        [Column("theme_id")]
        public int MissionThemeId { get; set; }
        public virtual MissionTheme? Theme { get; set; }

        // Many-to-Many Relationship with Skills
        public ICollection<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();
    }

}
