using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Entities
{
    public sealed class AnonymousChat : BaseChat
    {
        public ApplicationUser UserRequested { get; set; }
    }
}
