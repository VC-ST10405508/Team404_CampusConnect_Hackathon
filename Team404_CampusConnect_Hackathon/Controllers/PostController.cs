using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Controllers
{
    public class PostController : Controller
    {
        private readonly IRepository<User> _userInterface;
        private readonly IRepository<Post> _postInterface;
        private readonly ILogger<PostController> _logger;

        public PostController(IRepository<User> userInterface, IRepository<Post> postInterface, ILogger<PostController> logger)
        {
            _userInterface = userInterface;
            _postInterface = postInterface;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewPost()
        {
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to see posts. Please Login before trying to access other pages";
                return RedirectToAction("Index", "Home"); 
            }
            return View();
        }

        public async Task<IActionResult> All()
        {
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to see posts. Please Login before trying to access other pages";
                return RedirectToAction("Index", "Home");
            }
            var posts = await _postInterface.GetAllAsync();
            return View(posts);
        }
        [HttpPost]
        public async Task<IActionResult> NewPost(string title, string content, string category)
        {
            var user = HttpContext.Session.GetString("uID");
            if (string.IsNullOrEmpty(user))
            {
                //returning user to home page with error if they try to access a page without loggin in
                TempData["Error"] = "Must be logged in to perform actions and view other pages. Please log in";
                return RedirectToAction("Index", "Home");
            }
            var Username = HttpContext.Session.GetString("username");
            if (ModelState.IsValid)
            {
                Post post = new Post();
                post.Title = title;
                post.Content = content;
                post.Category = category;
                post.UserID = user;
                post.Username = Username;
                post.PostID = Guid.NewGuid().ToString(); // generate unique ID
                post.DateCreated = DateTime.Now;

                // Example: save to database
                await _postInterface.AddAsync(post);
                await _postInterface.SaveAsync();

                TempData["PostSuccess"] = "Your post has successfully been added :D";
                return RedirectToAction("All");
            }

            ViewBag.Error = "An error has occured well trying to save your post";
            return View();
        }
    }
}
