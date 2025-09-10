using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class Post
    {
        [Key]
        public string PostID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public string Username { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Category { get; set; }
    }
}
