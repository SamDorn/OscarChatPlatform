using OscarChatPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Repositories
{
    public interface IChatRoomQueueRepository
    {
        bool IsEmpty();
        Task Add(ChatRoomQueue chatRoomQueue);
        Task Remove(ChatRoomQueue chatRoomQueue);
        Task<ChatRoomQueue?> GetFirstCreated();
        Task<ChatRoomQueue?> GetByUser(ApplicationUser user);
    }
}
