using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<IActionResult> SendMessage(string message, string groupID)
        {
            //making sure that visitors cannot force join a group
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            //using a try catch to prevent program from crashing
            try
            {
                //creating the new message
                GroupChatMessage newMessage = new GroupChatMessage();
                newMessage.MessageID = "M" + Guid.NewGuid().ToString("N");
                newMessage.GroupID = groupID;
                newMessage.UserID = userId;
                newMessage.Content = message;

                //saving the group message to the database
                await _groupMessages.AddAsync(newMessage);
                await _groupMessages.SaveAsync();

                //returning the user back to the group page
                return RedirectToAction("SpecificGroup", new { id = groupID });

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(string GroupID, string Title, string Content)
        {
            //making sure that visitors cannot force join a group
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }

            //Getting the specific users group information so that we can check if they are a group member and if they are the owner:
            var membership = (await _userGroup.GetAllAsync()).FirstOrDefault(ug => ug.GroupID == GroupID && ug.UserID == userId);
            if (membership == null || membership.Role != "Owner")
            {
                TempData["Error"] = "Only group owners can post announcements.";
                return RedirectToAction("Index", "Home");
            }

            //creating the new announcements information
            var newAnnouncement = new GroupAnnouncement
            {
                AnnouncementID = "A" + Guid.NewGuid().ToString("N"),
                GroupID = GroupID,
                Title = Title,
                Content = Content,
                DateCreated = DateTime.Now
            };

            //saving the new announcement to the database
            await _groupAnnouncements.AddAsync(newAnnouncement);
            await _groupAnnouncements.SaveAsync();

            TempData["Success"] = "Announcement successfully added";
            return RedirectToAction("SpecificGroup", new { id = GroupID });
        }

        public async Task<IActionResult> CreateEvent(string groupID)
        {
            //making sure user is logged in before accessing the page
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.GroupID = groupID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(string groupID, string Title, string Description, DateTime EventDate, DateTime JoinDeadline)
        {
            // making sure that visitors cannot force join a group
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                //Getting the specific users group information so that we can check if they are a group member and if they are the owner:
                var membership = (await _userGroup.GetAllAsync()).FirstOrDefault(ug => ug.GroupID == groupID && ug.UserID == userId);
                if (membership == null || membership.Role != "Owner")
                {
                    //returning them to home page if they arent cause only owner can create event
                    TempData["Error"] = "Only group owners can make the events for the group.";
                    return RedirectToAction("Index", "Home");
                }

                //making sure the system is able to identify which group made the request - this is set when the page loads through ViewBag
                if (string.IsNullOrEmpty(groupID))
                {
                    //returning them to home page if they arent cause only owner can create event
                    TempData["Error"] = "Unable to identify group request is made from. Please contact support if you think this is a mistake";
                    return RedirectToAction("Index", "Home");
                }
                if (EventDate == null || JoinDeadline == null)
                {
                    TempData["Error"] = "Dates didnt set properly";
                    return RedirectToAction("Index", "Home");
                }
                //creating the group event entry to save to the database
                GroupEvent groupEvent = new GroupEvent();
                groupEvent.GroupID = groupID;
                groupEvent.EventID = "E" + Guid.NewGuid().ToString("N");
                groupEvent.Title = Title;
                groupEvent.Description = Description;
                groupEvent.JoinDeadline = JoinDeadline;
                groupEvent.EventDate = EventDate;

                //saving to the db
                await _groupEvent.AddAsync(groupEvent);
                await _groupEvent.SaveAsync();

                //returnign the group with a success smg to inform user it was successful
                TempData["Success"] = "Announcement successfully added";
                return RedirectToAction("SpecificGroup", new { id = groupID });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Unexpected error occured: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        //A method to display the current event that the user is trying to view
        public async Task<IActionResult> ViewEvent(string id)
        {
            //making sure user is logged in before they can view the page
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            //making sure the group does indeed exist and that the function recieved a legitmate request
            var evt = await _groupEvent.GetByIdAsync(id);
            if (evt == null)
            {
                TempData["Error"] = "Unable to identify group event";
                return RedirectToAction("Index", "Home");
            }
            try
            {
                // Check if user is a member of the group
                var membership = (await _userGroup.GetAllAsync())
                    .FirstOrDefault(ug => ug.GroupID == evt.GroupID && ug.UserID == userId);
                ViewBag.IsMember = membership != null;
                // Check if user already RSVPed
                var attendee = (await _eventAttendee.GetAllAsync()).FirstOrDefault(a => a.EventID == id && a.UserID == userId);
                if (attendee == null)
                {
                    return View(evt);
                }
                ViewBag.HasRSVPed = attendee != null;
                //Returning the event to the model so that we can work with it directly :
                return View(evt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occured" + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ViewEvent(string EventID, string groupID, string Leave)
        {
            var userId = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                //making sure the user is a membmer
                var membership = (await _userGroup.GetAllAsync())
                    .FirstOrDefault(ug => ug.GroupID == groupID && ug.UserID == userId);
                if (membership == null)
                {
                    TempData["Error"] = "Only group members can RSVP for this event.";
                    return RedirectToAction("Index", "Home");
                }
                //settting the member
                ViewBag.IsMember = membership != null;
                //checking if the user already RSVPed
                var existingRSVP = (await _eventAttendee.GetAllAsync())
                    .FirstOrDefault(ea => ea.EventID == EventID && ea.UserID == userId);


                if (!string.IsNullOrEmpty(Leave))
                {
                    // Leave Event
                    if (existingRSVP != null)
                    {
                        await _eventAttendee.DeleteAsync(existingRSVP.EventAttendeeID);
                        await _eventAttendee.SaveAsync();
                        ViewBag.SuccessMsg = "You have left the event.";
                    }
                }
                else
                {
                    //adding the rsvp
                    if (existingRSVP == null)
                    {
                        var attendee = new EventAttendee
                        {
                            EventAttendeeID = "RSVPn" + Guid.NewGuid().ToString("N"),
                            EventID = EventID,
                            UserID = userId
                        };
                        await _eventAttendee.AddAsync(attendee);
                        await _eventAttendee.SaveAsync();
                        ViewBag.SuccessMsg = "Successfully joined the event.";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "You have already joined this event.";
                    }
                }

                // Update the flag for the view
                ViewBag.HasRSVPed = !string.IsNullOrEmpty(Leave) ? false : true;

                var evt = await _groupEvent.GetByIdAsync(EventID);
                return View(evt);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Unexpected error occurred: " + ex.Message;
                var evt = await _groupEvent.GetByIdAsync(EventID);
                return View(evt);
            }
        }
    }
}
