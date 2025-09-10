using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Controllers
{
    public class UserController : Controller
    {
        //creating vars for the repositories that will be represented in this controller:
        public IRepository<User> _userInterface;
        public IRepository<Skill> _skillInterface;
        public IRepository<Sport> _sportInterface;
        public IUserService _userService;
        public ILogger<UserController> _logger;
        //constructor for the controller
        public UserController(IRepository<User> userInterface, IRepository<Skill> skillInterface, IRepository<Sport> sportInterface,IUserService userService, ILogger<UserController> logger)
        {
            _userInterface = userInterface;
            _skillInterface = skillInterface;
            _sportInterface = sportInterface;
            _userService = userService;
            _logger = logger;
        }


        //making the pages viewable without any input/forms
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            //logging the user out and changing the session info
            HttpContext.Session.SetString("uID", "");
            HttpContext.Session.SetString("username", "");
            HttpContext.Session.SetString("role", "");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        //making the pages viewable without any input/forms
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            return View();
        }
        //method for logging users into their account
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            string errMsg = "Invalid username or password";

            try
            {
                //data validation to make sure that fields recived are indeed filled in :
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ViewBag.errorMsg = "All fields are required. recived User: " + username + "  Recieved password: " + password;
                    return View();
                }
                //getting all the user details and then trying to find the specific user
                var user = await _userService.GetByUsernameAsync(username);

                if (user == null)
                {
                    //logging a warning in-case this was supposed to pass but it didnt
                    _logger.LogWarning("Failed login attempt for username {Username}", username);
                    ViewBag.errorMsg = errMsg + " error 1";
                    return View();
                }
                //Checking if the hashed passworded - stored in db - matchs password recieved:
                var passwordHasher = new PasswordHasher<User>();
                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    //setting up the session control for access control and authorization level:
                    HttpContext.Session.SetString("uID", user.UserID);
                    HttpContext.Session.SetString("username", user.UserName);
                    HttpContext.Session.SetString("role", user.role);

                    //logging that the user logged in successfully
                    _logger.LogInformation("User {Username} logged in successfully", username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for username {Username}", username);
                    ViewBag.errorMsg = errMsg;
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during login for username {Username}", username);
                ViewBag.errorMsg = "An unexpected error occurred. Please contact support.";
                return View();
            }
        }
        //method for creating a account
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string email, string? degree)
        {
            try
            {
                ViewBag.errorMsg = null;
                ViewBag.successMsg = null;
                //Making sure fields recieved are not empty:
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ViewBag.errorMsg = "All fields are required.";
                    return View();
                }
                //extra data validation to check valid email and password:
                if (!ValidEmail(email))
                {
                    ViewBag.errorMsg = "Please enter a valid email.";
                    return View();
                }

                if (!ValidPassword(password))
                {
                    ViewBag.errorMsg = "Password must be at least 8 characters long and contain  the following Characters: uppercase, lowercase, number, and special characters.";
                    return View();
                }
                //creating a user with the basic information they have provided
                User newUser = new User();
                newUser.Email = email;
                //hashing the password for additional security
                var passwordHasher = new PasswordHasher<User>();
                newUser.Password = passwordHasher.HashPassword(newUser, password);
                newUser.UserID = "U" + Guid.NewGuid().ToString();
                newUser.UserName = username;
                newUser.ProfilePhotoe = null;
                newUser.Degree = degree;

                await _userInterface.AddAsync(newUser);

                await _userInterface.SaveAsync();

                //setting up the session controll so that we can have authorization and access control systems in place:
                HttpContext.Session.SetString("uID", newUser.UserID);
                HttpContext.Session.SetString("username", newUser.UserName);
                HttpContext.Session.SetString("role", newUser.role);

                //logging the user's account creating to make sure the password is hashed and other details are correct
                _logger.LogInformation("User successfully registered. user info: " + newUser.ToString);

                ViewBag.successMsg = "You have successfully created an account";
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                ViewBag.errorMsg = ex.Message;
                return View();
            }
        }
            private Boolean ValidPassword(string password)
        {
            //Flag var that will return if the password is valid or not
            Boolean flag = false;

            // Check if password length is at least 8 characters (Nick Champsas, 2023):
            if (password.Length >= 8)
            {
                // Check if password contains at least one uppercase letter, one lowercase letter, one digit, and one special character
                bool hasUpper = password.Any(char.IsUpper);
                bool hasLower = password.Any(char.IsLower);
                bool hasDigit = password.Any(char.IsDigit);
                bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

                // If all conditions are met, set flag to true (Nick Champsas, 2023):
                if (hasUpper && hasLower && hasDigit && hasSpecialChar)
                {
                    flag = true;
                }
            }

            return flag;
        }


        private Boolean ValidEmail(string email)
        {
            Boolean validEmail = false;

            // Check if email contains '@' and '.' with basic validation (Nick Champsas, 2023):
            if (!string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains("."))
            {
                // Simple additional check for email format - must contain at least one '@' and '.' after '@'
                int atIndex = email.IndexOf('@');
                int dotIndex = email.IndexOf('.', atIndex);

                // Ensure the dot comes after the '@' and there's at least one character before '@' and after '.'
                if (atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.Length - 1)
                {
                    validEmail = true;
                }
            }

            return validEmail;
        }
    }
}
