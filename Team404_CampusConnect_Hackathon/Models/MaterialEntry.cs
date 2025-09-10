using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class MaterialEntry
    {
        [Key]
        public string EntryID { get; set; }

        // Links to User
        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        // Links to StudyMaterial
        [ForeignKey("StudyMaterial")]
        public string MaterialID { get; set; }
        public StudyMaterial StudyMaterial { get; set; }

        // Links to Subject
        [ForeignKey("Subject")]
        public string SubjectID { get; set; }
        public Subject Subject { get; set; }

        // Extra fields
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }
    }
}
