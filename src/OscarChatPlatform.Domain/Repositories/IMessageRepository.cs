using OscarChatPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task Add(Message message);
    }
}
