using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OscarChatPlatform.Web.Controllers
{
    [Route("chat")]
    public class ChatController : Controller
    {

        [Route("{chatId:guid}")]
        public IActionResult Index(Guid chatId)
        {

            return View();
        }
    }
}
