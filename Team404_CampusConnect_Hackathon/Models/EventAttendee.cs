using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class EventAttendee
    {
        [Key]
        public string EventAttendeeID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("GroupEvent")]
        public string EventID { get; set; }
        public GroupEvent GroupEvent { get; set; }

        public DateTime DateJoined { get; set; } = DateTime.Now;
    }
}
