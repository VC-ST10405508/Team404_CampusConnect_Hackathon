using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Group> _group;
        private readonly IRepository<UserGroup> _userGroup;
        private readonly IRepository<User> _user;
        private readonly IRepository<StudyMaterial> _studyMaterial;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IRepository<Group> group,
            IRepository<UserGroup> userGroup,
            IRepository<User> user,
            IRepository<StudyMaterial> studyMaterial,
            ILogger<HomeController> logger)
        {
            _group = group;
            _userGroup = userGroup;
            _user = user;
            _studyMaterial = studyMaterial;
            _logger = logger;
        }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public async Task<IActionResult> Material()
        {
            var materials = await _studyMaterial.GetAllAsync() ?? new List<StudyMaterial>();
            return View(materials);
        }

        public IActionResult CreateMaterial() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMaterial(StudyMaterial material, IFormFile upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await upload.CopyToAsync(fileStream);
                        }

                        material.FilePath = "/uploads/" + fileName;
                        material.FileType = Path.GetExtension(upload.FileName).ToLower();
                    }

                    await _studyMaterial.AddAsync(material);
                    await _studyMaterial.SaveAsync();

                    return RedirectToAction("Material");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating material");
                ModelState.AddModelError("", "An error occurred while creating the material.");
            }

            return View(material);
        }

        public async Task<IActionResult> EditProfile()
        {
            var currentUserId = HttpContext.Session.GetString("uID");
            var currentUser = !string.IsNullOrEmpty(currentUserId)
                ? (await _user.GetAllAsync()).FirstOrDefault(u => u.UserID == currentUserId)
                : null;

            if (currentUser == null)
                return RedirectToAction("Login");

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.GetByIdAsync(updatedUser.UserID);
                if (user == null) return NotFound();

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.UserSkills = updatedUser.UserSkills;
                user.Degree = updatedUser.Degree;

                await _user.UpdateAsync(user);
                await _user.SaveAsync();

                return RedirectToAction("Profile", "Home");
            }

            return View(updatedUser);
        }

        public async Task<IActionResult> Profile()
        {
            var currentUserId = HttpContext.Session.GetString("uID");
            var currentUser = !string.IsNullOrEmpty(currentUserId)
                ? (await _user.GetAllAsync()).FirstOrDefault(u => u.UserID == currentUserId)
                : null;

            if (currentUser == null)
                return RedirectToAction("Login");

            return View(currentUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}