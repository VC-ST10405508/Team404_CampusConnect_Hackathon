using System.ComponentModel.DataAnnotations;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class Sport
    {
        [Key]
        public string SportID { get; set; }
        public string SportName { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
