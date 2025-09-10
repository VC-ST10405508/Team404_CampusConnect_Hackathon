using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class UserGroup
    {
        [Key]
        public string UserGroupID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Group")]
        public string GroupID { get; set; }
        public Group Group { get; set; }

        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public string Role { get; set; }
    }
}
