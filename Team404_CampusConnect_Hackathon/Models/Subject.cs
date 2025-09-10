using System.ComponentModel.DataAnnotations;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class Subject
    {
        [Key]
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }

        // Optional: navigation to all material entries in this subject
        public List<MaterialEntry> MaterialEntries { get; set; } = new();
    }
}
