using System.Linq.Expressions;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Interface
{
    public interface IUserService
    {
        
        Task<User> GetByUsernameAsync(string userName);
    }
}
