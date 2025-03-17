﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    public sealed class Connection
    {
        public string Id { get; set; }
        public ApplicationUser User{ get; set; }
        public bool IsActive { get; set; }
    }
}
