using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    /// <summary>
    /// Entity that represents the chat between two users in anonymous mode
    /// </summary>
    public sealed class RandomChat
    {
        public string Id { get; set; }
        public List<ApplicationUser?> Users { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser? TerminatedByUser { get; set; }
        public DateTime LastActivityAt { get; set; }
    }
}
