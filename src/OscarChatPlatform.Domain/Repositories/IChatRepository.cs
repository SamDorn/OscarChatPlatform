using OscarChatPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Repositories
{
    public interface IChatRepository
    {
        Task<Chat?> GetById(string id);
        //Task<IEnumerable<string>> GetAllByUserId();
        Task<string> Add(Chat chat);
        Task RemoveById(string id);
        Task<string> GetChatWithOneUser(ApplicationUser user);
    }
}
