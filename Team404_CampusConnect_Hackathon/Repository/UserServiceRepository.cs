using Microsoft.EntityFrameworkCore;
using Team404_CampusConnect_Hackathon.Data;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Repository
{
    public class UserServiceRepository : IUserService
    {
        //creating an extra repository to aid with login feature performance. 
        private readonly AppDbContext _appDbContext;

        public UserServiceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //Performance improved - basically creating a SQL operation that will select the TOP(1) where the email or username exists.
        //before i was waiting for all the users to return in a list and then sorting through them so this is a much better option.
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Email == username);
        }
    }
}
