using OscarChatPlatform.Application.DTO;
using OscarChatPlatform.Domain.Entities;
using OscarChatPlatform.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Application.Services
{
    public class ChatService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IChatRoomQueueRepository _chatRoomQueueRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly INotificationService _notificationService;

        public ChatService(IUserRepository userRepository, INotificationService notificationService,
                        IConnectionRepository connectionRepository, IChatRepository chatRepository,
                        IChatRoomQueueRepository chatRoomQueueRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
            _connectionRepository = connectionRepository;
            _chatRepository = chatRepository;
            _chatRoomQueueRepository = chatRoomQueueRepository;
            _messageRepository = messageRepository;
        }
        #region Manage ConnectionId
        /// <summary>
        /// Returns all the connectionIds of the specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
        #endregion

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
                if (chatRoomQueue is null) await CreateChat(userId); // Is this a good approach?

                await _chatRoomQueueRepository.Remove(chatRoomQueue!);

                Chat chat = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    User = new List<ApplicationUser>() { user, chatRoomQueue!.User },
                    IsGroupChat = false,
                };
                await _chatRepository.Add(chat);

                IEnumerable<string> receiverConnectionIds = await this.GetConnectionIdsByUser(chatRoomQueue!.User);
                IEnumerable<string> senderConnectionIds = await this.GetConnectionIdsByUser(user);

                List<string> connectionIds = [.. receiverConnectionIds, .. senderConnectionIds];

                await _notificationService.SendChatIdToUsers(connectionIds, chat.Id);
            }

        }

        public async Task InsertMessage(string chatId, string message, string connectionId)
        {
            ApplicationUser sender = await _userRepository.GetByConnectionId(connectionId) ?? throw new Exception();

            Chat currentChat = await _chatRepository.GetById(chatId) ?? throw new Exception();

            Message currentMessage = new()
            {
                Id = Guid.NewGuid().ToString(),
                Chat = currentChat,
                Sender = sender,
                Content = message,
                SentAt = DateTime.Now
            };
            await _messageRepository.Add(currentMessage);

            IEnumerable<ApplicationUser> chatUsers = await _userRepository.GetUsersByChatId(chatId);
            List<string> connectionIds = [];

            foreach (ApplicationUser chatUser in chatUsers)
            {
                connectionIds.AddRange(await this.GetConnectionIdsByUser(chatUser));
            }

            await _notificationService.SendMessageToUsers(connectionIds, message, sender.Id);
        }

        public async Task DeleteChatRoomQueueIfExists(string connectionId)
        {
            ApplicationUser? user = await _userRepository.GetByConnectionId(connectionId);

            if (user is null)
                return;

            ChatRoomQueue? queue = await _chatRoomQueueRepository.GetByUser(user);

            // If the user is not present in any queue, just return
            if (queue is null)
                return;

            // Otherwise delete the queue so other users can't join it
            await _chatRoomQueueRepository.Remove(queue);

        }
        public async Task<IEnumerable<Message>> GetChatMessages(string chatId)
        {
            return await _messageRepository.GetAllByChatId(chatId);

        }

        public async Task<int> GetNumberActiveChats()
        {
            return 0;
        }
    }
}
