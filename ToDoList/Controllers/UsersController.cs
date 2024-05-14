using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersServices _users;
        private readonly IAccessControlService _auth;

        public UsersController(IUsersServices users, IAccessControlService auth)
        {
            _users = users;
            _auth = auth;
        }
        public IActionResult Index()
        {
            return View(_users.GetUsers());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                bool created = await _users.CreateUser(user);

                if (created) { return View(nameof(Index)); }
                else
                {
                    throw new Exception("Error creating user");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "invalid email")
                {
                    ModelState.AddModelError(key: "Email", errorMessage: "This email is already registered.");
                }
                return View();
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (_auth.VerifyIfUserLoggedIn())
            {
                return RedirectToAction("Index", "Tasks");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (_auth.VerifyIfUserLoggedIn())
            {
                return RedirectToAction("Index", "Tasks");
            }
            try
            {
                bool loggedIn = await _users.LoginUser(email, password);

                if (!loggedIn) { throw new Exception("Error loggin in"); }
                
                User? u = _users.FindUserByEmail(email);
                _auth.Login(u.UserID);

                return RedirectToAction("Index", "Tasks");
            } catch (Exception ex)
            {
                return View(nameof(Login));
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _auth.Logout();

            return RedirectToAction("Index", "Tasks");
        }
    }
}
