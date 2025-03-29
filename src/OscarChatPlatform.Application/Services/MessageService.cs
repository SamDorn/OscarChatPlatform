using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;

namespace OscarChatPlatform.Application.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly ConnectionService _connectionService;
        private readonly INotificationService _notificationService;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository,
                              IChatRepository chatRepository, ConnectionService connectionService,
                              INotificationService notificationService)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _connectionService = connectionService;
            _notificationService = notificationService;
        }

            public async Task InsertMessage(string chatId, string message, string userId)
            {
                ApplicationUser sender = await _userRepository.GetById(userId) ?? throw new Exception();

                RandomChat currentChat = await _chatRepository.GetById(chatId) ?? throw new Exception();

                Message currentMessage = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    RandomChat = currentChat,
                    SenderUser = sender,
                    Content = message,
                    SentAt = DateTime.Now
                };
                await _messageRepository.Add(currentMessage);

                IEnumerable<ApplicationUser> chatUsers = await _userRepository.GetUsersByChatId(chatId);
                List<string> connectionIds = new List<string>();

                foreach (ApplicationUser chatUser in chatUsers)
                {
                    connectionIds.AddRange(await _connectionService.GetConnectionIdsByUser(chatUser));
                }

                await _notificationService.SendMessageToUsers(connectionIds, message, sender.Id);
            }

        public async Task<IEnumerable<Message>> GetChatMessages(string chatId)
        {
            return await _messageRepository.GetAllByChatId(chatId);
        }
    }
}