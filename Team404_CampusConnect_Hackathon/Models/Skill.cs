using System.ComponentModel.DataAnnotations;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class Skill
    {
        [Key]
        public string SkillID { get; set; }
        public string SkillName { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
