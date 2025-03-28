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
    public class MessageRepository : IMessageRepository
    {
        public ApplicationDbContext _dbContext;
        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Message message)
        {
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetAllByChatId(string chatId)
        {
            return await _dbContext.Messages
                .Include(m => m.SenderUser)
                .Include(m => m.RandomChat)
                .Where(m => m.RandomChat.Id == chatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
