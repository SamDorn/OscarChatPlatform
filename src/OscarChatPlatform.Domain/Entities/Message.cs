using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    public sealed class Message
    {
        public string Id { get; set; }
        public RandomChat? RandomChat { get; set; }
        public RandomChat? StandardChat { get; set; }
        public ApplicationUser SenderUser { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
