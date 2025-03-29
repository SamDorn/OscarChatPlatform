using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;

namespace OscarChatPlatform.Application.Services
{
    public class ChatRoomService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IChatRoomQueueRepository _chatRoomQueueRepository;
        private readonly IUserRepository _userRepository;
        private readonly ConnectionService _connectionService;
        private readonly INotificationService _notificationService;

        public ChatRoomService(IChatRepository chatRepository, IChatRoomQueueRepository chatRoomQueueRepository,
                               IUserRepository userRepository, ConnectionService connectionService,
                               INotificationService notificationService)
        {
            _chatRepository = chatRepository;
            _chatRoomQueueRepository = chatRoomQueueRepository;
            _userRepository = userRepository;
            _connectionService = connectionService;
            _notificationService = notificationService;
        }

        public async Task CreateChat(string userId)
        {
            ApplicationUser user = await _userRepository.GetById(userId) ?? throw new Exception();

            bool isQueueEmpty = _chatRoomQueueRepository.IsEmpty();

            if (isQueueEmpty)
            {
                ChatRoomQueue chatRoomQueue = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    User = user,
                    CreatedAt = DateTime.Now
                };
                await _chatRoomQueueRepository.Add(chatRoomQueue);
            }
            else
            {
                ChatRoomQueue? chatRoomQueue = await _chatRoomQueueRepository.GetFirstCreated();
                if (chatRoomQueue is null) await CreateChat(userId);

                await _chatRoomQueueRepository.Remove(chatRoomQueue!);

                RandomChat chat = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Users = new List<ApplicationUser?>() { user, chatRoomQueue!.User }
                };
                await _chatRepository.Add(chat);

                IEnumerable<string> receiverConnectionIds = await _connectionService.GetConnectionIdsByUser(chatRoomQueue!.User);
                IEnumerable<string> senderConnectionIds = await _connectionService.GetConnectionIdsByUser(user);

                List<string> connectionIds = new(receiverConnectionIds.Concat(senderConnectionIds));

                await _notificationService.SendChatIdToUsers(connectionIds, chat.Id);
            }
        }

        public async Task TerminateChat(string chatId, string terminatedByUserId)
        {
            RandomChat chat = await _chatRepository.GetById(chatId) ?? throw new Exception();
            chat.TerminatedByUser = await _userRepository.GetById(terminatedByUserId) ?? throw new Exception("Utente non trovato");

            await _chatRepository.Update(chat);

            IEnumerable<ApplicationUser> chatUsers = await _userRepository.GetUsersByChatId(chatId);
            List<string> connectionIds = new List<string>();

            foreach (ApplicationUser chatUser in chatUsers)
            {
                connectionIds.AddRange(await _connectionService.GetConnectionIdsByUser(chatUser));
            }

            await _notificationService.TerminateChat(connectionIds, terminatedByUserId);
        }

        public async Task DeleteChatRoomQueueIfExists(string connectionId)
        {
            ApplicationUser? user = await _userRepository.GetByConnectionId(connectionId);

            if (user is null)
                return;

            ChatRoomQueue? queue = await _chatRoomQueueRepository.GetByUser(user);

            if (queue is null)
                return;

            await _chatRoomQueueRepository.Remove(queue);
        }

        public async Task<string?> GetTerminatedUserByChatId(string id)
        {
            RandomChat chat = await _chatRepository.GetById(id) ?? throw new Exception();

            return chat.TerminatedByUser?.Id;

        }
    }
}