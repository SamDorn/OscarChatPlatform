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
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Add(RandomChat chat)
        {
            await _dbContext.AddAsync(chat);
            await _dbContext.SaveChangesAsync();
            return chat.Id;
        }

        public async Task<RandomChat?> GetById(string id)
        {
            return await _dbContext.RandomChats
                .Include(c => c.TerminatedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<string> GetChatWithOneUser(ApplicationUser user)
        {
            RandomChat? chat = _dbContext.RandomChats
                .Where(x => x.Users.Contains(user))
                .FirstOrDefault(c => c.Users.Count == 1);

            if (chat is null)
            {
                chat = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Users = new List<ApplicationUser?>() { user }
                };
                await this.Add(chat);
            }
            return chat.Id;
        }

        public Task RemoveById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(RandomChat chat)
        {
            _dbContext.Update(chat);
            await _dbContext.SaveChangesAsync();
        }
    }
}
