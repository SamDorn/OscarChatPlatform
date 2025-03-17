using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OscarChatPlatform.Application.Services;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Web.Models;

namespace OscarChatPlatform.Web.Controllers
{
    [Route("chat")]
    public class ChatController : Controller
    {
        public ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [Route("{chatId:guid}")]
        public async Task<IActionResult> Index(Guid chatId)
        {
            IEnumerable<Message> messages = await _chatService.GetChatMessages(chatId.ToString());


            ChatViewModel model = new ChatViewModel()
            {
                UserId = HttpContext.Request.Cookies["UserId"] ?? "",
                Messages = messages
            };


            return View(model);
        }
    }
}
