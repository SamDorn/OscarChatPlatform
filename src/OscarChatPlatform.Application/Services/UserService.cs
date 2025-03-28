using Microsoft.AspNetCore.Identity;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;

namespace OscarChatPlatform.Application.Services
{
    public sealed class UserService
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _tokenProvider = tokenProvider;
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

            string token = _tokenProvider.GenerateToken(user);

            return token;
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
