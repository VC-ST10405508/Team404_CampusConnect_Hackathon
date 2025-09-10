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
        private readonly IRepository<Comment> _commentInterface;
        private readonly ILogger<PostController> _logger;

        public PostController(IRepository<User> userInterface, IRepository<Post> postInterface, IRepository<Comment> commentInterface, ILogger<PostController> logger)
        {
            _userInterface = userInterface;
            _postInterface = postInterface;
            _commentInterface = commentInterface;
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
                TempData["Error"] = "Must be logged in to see posts. Please Login before trying to access other pages";
                return RedirectToAction("Index", "Home");
            }

            var posts = await _postInterface.GetAllAsync();

            // Fetch all comments
            var comments = await _commentInterface.GetAllAsync();

            // Pass both to the view using a ViewModel
            var viewModel = posts.Select(p => new PostWithCommentsViewModel
            {
                Post = p,
                Comments = comments.Where(c => c.PostID == p.PostID).OrderByDescending(c => c.DateCreated).ToList()
            }).ToList();

            return View(viewModel);
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
        [HttpPost]
        public async Task<IActionResult> Add(string postId, string content)
        {
            var user = HttpContext.Session.GetString("uID");
            var username = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(user))
            {
                TempData["Error"] = "You must be logged in to comment.";
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(content))
            {
                var comment = new Comment
                {
                    CommentID = Guid.NewGuid().ToString(),
                    PostID = postId,
                    UserID = user,
                    Username = username,
                    Content = content,
                    DateCreated = DateTime.Now
                };

                await _commentInterface.AddAsync(comment);
                await _commentInterface.SaveAsync();

                TempData["CommentSuccess"] = "Comment added!";
            }
            else
            {
                TempData["Error"] = "Comment cannot be empty.";
            }

            return RedirectToAction("All", "Post");
        }
    }
}

