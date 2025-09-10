using System.ComponentModel.DataAnnotations;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class Group
    {
        [Key]
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public List<UserGroup> UserGroups { get; set; } = new();          
        public List<GroupAnnouncement> Announcements { get; set; } = new();
        public List<GroupEvent> Events { get; set; } = new();
        public List<GroupChatMessage> GroupChatMessages { get; set; } = new();
    }
}
