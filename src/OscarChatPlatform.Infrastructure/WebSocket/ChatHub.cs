using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OscarChatPlatform.Application;
using OscarChatPlatform.Application.Services;

namespace OscarChatPlatform.Infrastructure.WebSocket
{
    [Authorize]
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
            await _chatService.InsertMessage(chatId, message, Context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
        }

        public async Task TerminateChat(string chatId, string terminatedByUserId)
        {
            await _chatService.TerminateChat(chatId, terminatedByUserId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _chatService.DeleteChatRoomQueueIfExists(Context.ConnectionId);
            // Delete the record from the connection table
            await _chatService.DeleteConnectionId(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
