using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team404_CampusConnect_Hackathon.Models
{
    public class User
    {
        [Key]
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string? ProfilePhotoe { get; set; }
        public string? Degree {  get; set; }
        //automaticall fills in the date and time
        public DateTime? Created { get; set; } = DateTime.Now;
        //by default a user is created as a student unless overrided
        public string role { get; set; } = "Student";

        public string?  UserSkills { get; set; } 
        public string? UserSports { get; set; }
    }
}
