using Microsoft.EntityFrameworkCore;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Infrastructure.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ConnectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Add(Connection connection)
        {
            await _dbContext.AddAsync(connection);
            await _dbContext.SaveChangesAsync();
            return connection.Id.ToString();
        }

        public async Task<IEnumerable<string>> GetAllByUser(ApplicationUser user)
        {
            return await _dbContext.Connections
                .Include(c => c.User)
                .Where(c => c.User == user)
                .Select(c => c.Id)
                .ToListAsync();
        }

        public Task<string?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveById(string connectionId)
        {
            Connection connection = new() { Id = connectionId };
            _dbContext.Connections.Attach(connection);
            _dbContext.Connections.Remove(connection);
            await _dbContext.SaveChangesAsync();
        }
    }
}
