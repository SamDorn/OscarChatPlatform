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

        public async Task<string> Add(Chat chat)
        {
            await _dbContext.AddAsync(chat);
            await _dbContext.SaveChangesAsync();
            return chat.Id;
        }

        public async Task<Chat?> GetById(string id)
        {
            return await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<string> GetChatWithOneUser(ApplicationUser user)
        {
            Chat? chat = _dbContext.Chats.Where(x => x.User.Contains(user)).FirstOrDefault(c => c.User.Count == 1);

            if(chat is null)
            {
                chat = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    User = new List<ApplicationUser>() { user }
                };
                await this.Add(chat);
            }
            return chat.Id;
        }

        public Task RemoveById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
