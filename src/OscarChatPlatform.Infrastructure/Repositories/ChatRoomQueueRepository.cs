using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Infrastructure.Repositories
{
    public class ChatRoomQueueRepository : IChatRoomQueueRepository
    {
        public ApplicationDbContext _dbContext;

        public ChatRoomQueueRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(ChatRoomQueue chatRoomQueue)
        {
            await _dbContext.AddAsync(chatRoomQueue);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<ChatRoomQueue?> GetFirstCreated()
        {
            return await _dbContext.ChatRoomQueues
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Return True if the queue is not empty: otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _dbContext.ChatRoomQueues.IsNullOrEmpty();
        }

        public async Task Remove(ChatRoomQueue chatRoomQueue)
        {
            _dbContext.Remove(chatRoomQueue);
            await _dbContext.SaveChangesAsync();
        }
    }
}
