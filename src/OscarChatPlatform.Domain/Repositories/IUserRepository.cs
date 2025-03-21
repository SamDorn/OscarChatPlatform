using OscarChatPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetById(string id);
        Task<ApplicationUser?> GetByConnectionId(string connectionId);
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<string> Add(ApplicationUser user);
        Task Remove();
        Task<IEnumerable<ApplicationUser>> GetUsersByChatId(string chatId);
        Task<int> GetNumberOnlineUsers();


    }
}
