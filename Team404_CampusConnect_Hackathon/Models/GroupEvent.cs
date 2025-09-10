using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class GroupEvent
    {
        [Key]
        public string EventID { get; set; }

        [ForeignKey("Group")]
        public string GroupID { get; set; }
        public Group Group { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }

        // Deadline for joining
        public DateTime JoinDeadline { get; set; }

        // Members who joined
        public List<EventAttendee> Attendees { get; set; } = new();
    }
}
