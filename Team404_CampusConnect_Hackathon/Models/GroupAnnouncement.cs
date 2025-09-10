using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class GroupAnnouncement
    {
        [Key]
        public string AnnouncementID { get; set; }

        [ForeignKey("Group")]
        public string GroupID { get; set; }
        public Group Group { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
