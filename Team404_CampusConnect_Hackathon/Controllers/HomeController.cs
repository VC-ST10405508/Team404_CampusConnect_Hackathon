using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Group> _group;
        private readonly IRepository<UserGroup> _userGroup;
        private readonly IRepository<User> _user;

        public HomeController(IRepository<Group> group, IRepository<UserGroup> userGroup, IRepository<User> user, IRepository<GroupChatMessage> groupMessages, IRepository<GroupAnnouncement> groupAnnouncements, IRepository<EventAttendee> eventAttendee, IRepository<GroupEvent> groupEvent, IGroupWithDetails groupDetails)
        {
            _group = group;
            _userGroup = userGroup;
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> EditProfile()
        {
            var currentUserId = HttpContext.Session.GetString("uID");

            Team404_CampusConnect_Hackathon.Models.User currentUser = null;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                var users = await _user.GetAllAsync();
                currentUser = users.FirstOrDefault(u => u.UserID == currentUserId);
            }

            if (currentUser == null)
            {
                // Redirect to login or error page if user not found
                return RedirectToAction("Login");
            }

            // Pass only the current user to the view
            return View(currentUser);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                // Get the existing user from DB
                var user = await _user.GetByIdAsync(updatedUser.UserID);
                if (user == null)
                {
                    return NotFound();
                }

                // Update fields
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.UserSkills = updatedUser.UserSkills; // string like "C#, Java"
                user.Degree = updatedUser.Degree;

                

                // Save changes
                await _user.UpdateAsync(user); // update tracked entity
                await _user.SaveAsync();       // commit changes to DB

                return RedirectToAction("Profile", "Home");
            }

            // If something fails, reload the form with data
            return View(updatedUser);
        }



        // Add 'async' and return Task<IActionResult>
        public async Task<IActionResult> Profile()
        {
            // Get the current user's ID from session
            var currentUserId = HttpContext.Session.GetString("uID");

            Team404_CampusConnect_Hackathon.Models.User currentUser = null;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                // Get all users and find the one that matches the current user's ID
                var users = await _user.GetAllAsync(); 
                currentUser = users.FirstOrDefault(u => u.UserID == currentUserId);
            }

            if (currentUser == null)
            {
                // Optional: redirect to login if no current user is found
                return RedirectToAction("Login");
            }

            // Pass only the current user to the view
            return View(currentUser);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
