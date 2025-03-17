using Microsoft.AspNetCore.SignalR;
using OscarChatPlatform.Application;
using OscarChatPlatform.Application.DTO;
using OscarChatPlatform.Application.Services;

namespace OscarChatPlatform.Infrastructure.WebSocket
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task AppendConnectionIdToUser(string userId)
        {
            // Insert the connectionId to the connection table
            await _chatService.InsertConnectionId(userId, Context.ConnectionId);
        }
        public async Task JoinChat(string userId)
        {
            await _chatService.CreateChat(userId);
        }
        public async Task SendMessage(string chatId, string message)
        {
            await _chatService.InsertMessage(chatId, message, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Delete the record from the connection table
            await _chatService.DeleteConnectionId(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
