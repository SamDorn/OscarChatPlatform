using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    /// <summary>
    /// Entity that represent the standard chat between two or more people
    /// </summary>
    public sealed class StandardChat
    {
        /// <summary>
        /// Unique identifier of the chat
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Participants of the chat
        /// </summary>
        public List<ApplicationUser?> User { get; set; }

        /// <summary>
        /// Messages of the chat
        /// </summary>
        public List<Message> Message { get; set; }

        /// <summary>
        /// Represents when the chat was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
