namespace Team404_CampusConnect_Hackathon.Models
{
    public class PostWithCommentsViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
