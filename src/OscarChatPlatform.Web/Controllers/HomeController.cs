using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ChatService _chatService;
        private readonly IViewLocalizer _localizer;

        public HomeController(ILogger<HomeController> logger, UserService userService,
                              IViewLocalizer localizer, ChatService chatService)
        {
            _logger = logger;
            _userService = userService;
            _localizer = localizer;
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            if(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value is not null)
            {
                return RedirectToAction("home");
            }

            return View();
        }

        [Authorize]
        [HttpGet("home")]
        public async Task<IActionResult> Home()
        {
            HomeViewModel model = new()
            {
                UserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value ?? string.Empty,
                IsNewUser = bool.Parse(HttpContext.Request.Cookies["IsNewUser"] ?? "false"),
                ActiveChats = await _chatService.GetNumberActiveChats(),
                OnlineUsers = await _userService.GetNumberActiveUser()
            };
            HttpContext.Response.Cookies.Delete("IsNewUser");

            return View(model);
        }

        [HttpPost("createGuestUser")]
        public async Task<IActionResult> CreateGuestUser()
        {
            // Create the new anonymous user
            string token = await _userService.CreateUser();

            HttpContext.Response.Cookies.Append("token", token, new CookieOptions()
            {
                Expires = DateTimeOffset.MaxValue,
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
