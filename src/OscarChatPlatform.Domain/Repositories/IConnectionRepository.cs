using OscarChatPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Domain.Repositories
{
    public interface IConnectionRepository
    {
        Task<string> GetById(string id);
        Task<IEnumerable<string>> GetAllByUser(ApplicationUser user);
        Task<string> Add(Connection connection);
        Task RemoveById(string connectionId);
    }
}
