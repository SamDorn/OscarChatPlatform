using Microsoft.AspNetCore.Identity;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Application.Services
{
    public sealed class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<string> CreateUser()
        {
            string userId = Guid.NewGuid().ToString();

            ApplicationUser user = new()
            {
                Id = userId,
                UserName = $"Guest_{userId}",
                IsAnonymous = true,
                CreatedAt = DateTime.Now
            };
            await _userManager.CreateAsync(user);

            return user.Id;
        }

        public async Task<int> GetNumberActiveUser()
        {
            return await _userRepository.GetNumberOnlineUsers();
        }

        public async Task<bool> IsValidUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) 
                return false;

            return await _userRepository.GetById(userId) != null;
        }
    }
}
