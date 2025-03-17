using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    public sealed class ChatRoomQueue
    {
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
