using Microsoft.EntityFrameworkCore;
using Team404_CampusConnect_Hackathon.Data;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Repository
{
    public class GroupDetailsRepository : IGroupWithDetails
    {
        private readonly AppDbContext _context;

        public GroupDetailsRepository(AppDbContext context)
        {
            _context = context;
        }
        //we essentially need to make sure that when a group is called upon, it will load all the currrect data such as events,
        //announcements and chats
        public async Task<Group> GetGroupWithDetailsAsync(string id)
        {
            return await _context.Groups
                .Include(g => g.UserGroups)
                    .ThenInclude(ug => ug.User)
                .Include(g => g.Announcements)
                .Include(g => g.Events)
                .Include(g => g.GroupChatMessages)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g => g.GroupID == id);
        }
    }
}
