using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public bool IsAnonymous { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RandomChat> RandomChats { get; set; }
        public List<StandardChat> StandardChats { get; set; }
    }
}
