using System.ComponentModel.DataAnnotations;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class StudyMaterial
    {
        [Key]
        public string MaterialID { get; set; }
        public string Title { get; set; }
        public byte[] MaterialData { get; set; }
        public DateTime UploadedAt { get; set; }
        public List<MaterialEntry> MaterialEntries { get; set; } = new();
    }
}
