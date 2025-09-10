using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class StudyMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Save the uploaded file path
        public string? FilePath { get; set; }

        // Optional: store file type for display logic
        public string? FileType { get; set; } 
    }
}