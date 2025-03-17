﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    public sealed class Chat
    {
        public string Id { get; set; }
        public List<ApplicationUser> User { get; set; }
        public List<Message> Message { get; set; }
        public bool IsGroupChat { get; set; }
    }
}
