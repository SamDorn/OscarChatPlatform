using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;

namespace OscarChatPlatform.Application.Services
{


    public class ConnectionService
    {
        private readonly IConnectionRepository _connectionRepository;
        private readonly IUserRepository _userRepository;

        public ConnectionService(IConnectionRepository connectionRepository, IUserRepository userRepository)
        {
            _connectionRepository = connectionRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<string>> GetConnectionIdsByUser(ApplicationUser user)
        {
            return await _connectionRepository.GetAllByUser(user);
        }

        public async Task InsertConnectionId(string userId, string connectionId)
        {
            ApplicationUser user = await _userRepository.GetById(userId) ?? throw new Exception("User non puó essere null");
            Connection connection = new()
            {
                Id = connectionId,
                User = user,
                IsActive = true
            };
            await _connectionRepository.Add(connection);
        }

        public async Task DeleteConnectionId(string connectionId)
        {
            await _connectionRepository.RemoveById(connectionId);
        }
    }
}