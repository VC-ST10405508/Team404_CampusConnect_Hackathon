using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Controllers
{
    public class GroupController : Controller
    {
        private readonly IRepository<Group> _group;
        private readonly IRepository<UserGroup> _userGroup;
        private readonly IRepository<User> _user;
        private readonly IRepository<GroupChatMessage> _groupMessages;
        private readonly IRepository<GroupAnnouncement> _groupAnnouncements;
        private readonly IRepository<EventAttendee> _eventAttendee;
        private readonly IRepository<GroupEvent> _groupEvent;
        private readonly IGroupWithDetails _groupWithDetails;

        public GroupController(IRepository<Group> group, IRepository<UserGroup> userGroup, IRepository<User> user, IRepository<GroupChatMessage> groupMessages, IRepository<GroupAnnouncement> groupAnnouncements, IRepository<EventAttendee> eventAttendee, IRepository<GroupEvent> groupEvent, IGroupWithDetails groupDetails)
        {
            _group = group;
            _userGroup = userGroup;
            _user = user;
            _groupMessages = groupMessages;
            _groupAnnouncements = groupAnnouncements;
            _eventAttendee = eventAttendee;
            _groupEvent = groupEvent;
            _groupWithDetails = groupDetails;
        }
        public IActionResult CreateGroup()
        {
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateGroup(string groupName, string description)
        {
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(description))
            {
                ViewBag.ErrorMsg = "Please fill in the group name and description.";
                return View();
            }
            try
            {
                //creating a new group and adding that group to the current user's groups.
                Group group = new Group();
                group.GroupID = "G" + Guid.NewGuid().ToString("N");
                group.GroupName = groupName;
                group.Description = description;

                //Since current user creates the group they get assigned owner role.
                UserGroup userGroup = new UserGroup();
                userGroup.GroupID = group.GroupID;
                userGroup.UserID = user;
                userGroup.User = await _user.GetByIdAsync(user);
                userGroup.Group = group;
                userGroup.UserGroupID = "U" + Guid.NewGuid().ToString("N");
                userGroup.Role = "Owner";
                
                //saving the information into the database
                await _group.AddAsync(group);
                await _group.SaveAsync();
                await _userGroup.AddAsync(userGroup);
                await _userGroup.SaveAsync();
                
                //success msg
                ViewBag.SuccessMsg = "Group successfully created";
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Unexpected error occured";
                return View();
            }
            return View();
        }
        public IActionResult EditGroup()
        {
            return View();
        }
        public async Task<IActionResult> AllGroups()
        {
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            var groups = await _group.GetAllAsync();
            return View(groups);
        }
        public IActionResult GroupChatMessage()
        {
            return View();
        }
        public IActionResult GroupAnnouncement()
        {
            return View();
        }
        public async Task<IActionResult> SpecificGroup(string id)
        {
            //getting the current user ID to make sure a user is logged in
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            // Load the group and all its related data - the custom group Interface is used here
            var group = await _groupWithDetails.GetGroupWithDetailsAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> JoinGroup(string groupID)
        {
            //making sure the function doesnt continue if they arent logged in:
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Home");

            // Check if already a member to ensure that an existing member didnt try activate this function again.
            var existing = (await _userGroup.GetAllAsync())
                .FirstOrDefault(ug => ug.GroupID == groupID && ug.UserID == userId);
            if (existing == null)
            {
                //adding the user to the group
                UserGroup newMember = new UserGroup
                {
                    UserGroupID = "U" + Guid.NewGuid().ToString("N"),
                    GroupID = groupID,
                    UserID = userId,
                    Role = "Member",
                    JoinedDate = DateTime.Now
                };
                //adding the information to the data and making sure it saves
                await _userGroup.AddAsync(newMember);
                await _userGroup.SaveAsync();
            }
            //it redirects thems to the page so that it can update
            return RedirectToAction("SpecificGroup", new { id = groupID });
        }
        public IActionResult JoinGroup()
        {
            return View();
        }
    }
}
