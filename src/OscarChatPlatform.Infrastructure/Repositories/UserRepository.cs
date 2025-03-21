using Microsoft.AspNetCore.Identity;
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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetById(string id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<IEnumerable<ApplicationUser>> GetAll()
        {
            throw new NotImplementedException();
        }


        public async Task<string> Add(ApplicationUser user)
        {
            await _userManager.CreateAsync(user);
            return user.Id;
        }
        public Task Remove()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> GetByConnectionId(string connectionId)
        {
            return await _dbContext.Connections
                .Include(c => c.User)
                .Where(c => c.Id == connectionId)
                .Select(c => c.User)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<ApplicationUser>> GetUsersByChatId(string chatId)
        {
            return await _dbContext.Users
                .Where(u => u.Chat.Any(c => c.Id == chatId))
                .ToListAsync();
        }

        public async Task<int> GetNumberOnlineUsers()
        {
            var a = await _dbContext.Connections
                .GroupBy(c => c.User)
                .Select(g => g.First())
                .CountAsync();
            return a;
        }
    }
}
