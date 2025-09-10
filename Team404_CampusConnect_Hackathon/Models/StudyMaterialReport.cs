using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class StudyMaterialReport
    {
        [Key]
        public string ReportID { get; set; }

        [ForeignKey("StudyMaterial")]
        public string MaterialID { get; set; }
        public StudyMaterial StudyMaterial { get; set; }

        public DateTime DateReported { get; set; } = DateTime.Now;
        public string Category { get; set; }  
        public string Description { get; set; } 
    }
}
