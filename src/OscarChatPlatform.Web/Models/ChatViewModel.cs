using OscarChatPlatform.Domain.Entities;

namespace OscarChatPlatform.Web.Models
{
    public class ChatViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public string? TerminatedByUserId { get; internal set; }
    }
}
