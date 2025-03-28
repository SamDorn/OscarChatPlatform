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
        Task<RandomChat?> GetById(string id);
        //Task<IEnumerable<string>> GetAllByUserId();
        Task<string> Add(RandomChat chat);
        Task RemoveById(string id);
        Task<string> GetChatWithOneUser(ApplicationUser user);
        Task Update(RandomChat chat);
    }
}
