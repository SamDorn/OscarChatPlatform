using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OscarChatRoom.Web.Controllers
{
    [Route("chat")]
    public class ChatController : Controller
    {

        [Route("")]
        public IActionResult Index()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionId")))
                HttpContext.Session.SetString("SessionId", Convert.ToString(Guid.NewGuid())!);

            ViewBag.SessionId = HttpContext.Session.GetString("SessionId");
            return View();
        }
    }
}
