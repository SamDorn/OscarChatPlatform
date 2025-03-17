using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using OscarChatPlatform.Application;
using OscarChatPlatform.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Infrastructure.WebSocket
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRNotificationService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendChatIdToUsers(IReadOnlyList<string> connectionIds, string chatId)
        {
            await _hubContext.Clients.Clients(connectionIds).SendAsync("JoinRoom", chatId);
        }

        public async Task SendMessageToUsers(IReadOnlyList<string> connectionIds, string message, string senderId)
        {
            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", message, senderId);
        }
    }
}
