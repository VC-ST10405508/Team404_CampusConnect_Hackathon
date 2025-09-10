using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class GroupChatMessage
    {
        [Key]
        public string MessageID { get; set; }

        
        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        
        [ForeignKey("Group")]
        public string GroupID { get; set; }
        public Group Group { get; set; }

        public string Content { get; set; }
        public DateTime DateSent { get; set; } = DateTime.Now;
    }
}
