using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{

    public class Comment
    {
        //a class for adding comments
        [Key]
        public string CommentID { get; set; }
        [ForeignKey("Post")]
        public string PostID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
   
}
