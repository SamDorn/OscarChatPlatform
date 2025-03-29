using OscarChatPlatform.Application.Services;
using OscarChatPlatform.Domain.Entities;

namespace OscarChatPlatform.Application.Services
{
    public class ChatService
    {
        private readonly ConnectionService _connectionService;
        private readonly ChatRoomService _chatRoomService;
        private readonly MessageService _messageService;

        public ChatService(ConnectionService connectionService, ChatRoomService chatRoomService, MessageService messageService)
        {
            _connectionService = connectionService;
            _chatRoomService = chatRoomService;
            _messageService = messageService;
        }

        public async Task<IEnumerable<string>> GetConnectionIdsByUser(ApplicationUser user)
        {
            return await _connectionService.GetConnectionIdsByUser(user);
        }

        public async Task InsertConnectionId(string userId, string connectionId)
        {
            await _connectionService.InsertConnectionId(userId, connectionId);
        }

        public async Task DeleteConnectionId(string connectionId)
        {
            await _connectionService.DeleteConnectionId(connectionId);
        }

        public async Task CreateChat(string userId)
        {
            await _chatRoomService.CreateChat(userId);
        }

        public async Task InsertMessage(string chatId, string message, string userId)
        {
            await _messageService.InsertMessage(chatId, message, userId);
        }

        public async Task DeleteChatRoomQueueIfExists(string connectionId)
        {
            await _chatRoomService.DeleteChatRoomQueueIfExists(connectionId);
        }

        public async Task<IEnumerable<Message>> GetChatMessages(string chatId)
        {
            return await _messageService.GetChatMessages(chatId);
        }

        public async Task<int> GetNumberActiveChats()
        {
            return 0;
        }

        public async Task TerminateChat(string chatId, string terminatedByUserId)
        {
            await _chatRoomService.TerminateChat(chatId, terminatedByUserId);
        }

        public async Task<string?> GetTerminatedUserByChatId(string id)
        {
            return await _chatRoomService.GetTerminatedUserByChatId(id);
        }
    }
}