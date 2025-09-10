using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Interface
{
    public interface IGroupWithDetails
    {
        //a custom interface that will bind certain tables to the group page - expanded in repository 
        Task<Group> GetGroupWithDetailsAsync(string id);
    }
}
