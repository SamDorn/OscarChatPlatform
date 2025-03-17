using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OscarChatPlatform.Application.Services;
using OscarChatPlatform.Web.Models;
using System.Diagnostics;

namespace OscarChatPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly IViewLocalizer _localizer;

        public HomeController(ILogger<HomeController> logger, UserService userService, IViewLocalizer localizer)
        {
            _logger = logger;
            _userService = userService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Request.Cookies["UserId"] ?? string.Empty;

            if (await _userService.IsValidUser(userId))
                return RedirectToAction("Home");

            return View();
        }

        [HttpGet("home")]
        public async Task<IActionResult> Home()
        {

            string userId = HttpContext.Request.Cookies["UserId"] ?? string.Empty;

            // If the userId is empty or it's not a valid user redirect to the login page
            if (string.IsNullOrEmpty(userId) || !await _userService.IsValidUser(userId))
            {
                return RedirectToAction("Index");
            }

            HomeViewModel model = new()
            {
                IsNewUser = bool.Parse(HttpContext.Request.Cookies["IsNewUser"] ?? "false")
            };

            return View(model);
        }

        [HttpPost("createGuestUser")]
        public async Task<IActionResult> CreateGuestUser()
        {
            // Create the new anonymous user
            string userId = await _userService.CreateUser();

            // Append the UserId in the cookie
            HttpContext.Response.Cookies.Append("UserId", userId, new CookieOptions() 
            { 
                Expires = DateTimeOffset.MaxValue, 
                Secure = true
            });

            HttpContext.Response.Cookies.Append("IsNewUser", "true", new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddMinutes(1),
                Secure = true
            });

            // Redirects to the home page
            return Json(new { redirectUrl = Url.Action("Home", "Home") });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
