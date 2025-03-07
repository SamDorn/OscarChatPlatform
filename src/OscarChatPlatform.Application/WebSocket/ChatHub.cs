using Ganss.Xss;
using Microsoft.AspNetCore.SignalR;

namespace OscarChatRoom.Application.WebSocket
{
    public class ChatHub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly HtmlSanitizer _sanitizer;

        public ChatHub(IConnectionManager connectionManager, HtmlSanitizer sanitizer)
        {
            _connectionManager = connectionManager;
            _sanitizer = sanitizer;
            _sanitizer.AllowedTags.Clear();
        }

        public async Task SendMessage(UserChatHubDTO user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, _sanitizer.Sanitize(message));

        }
        public void RegisterUser(UserChatHubDTO user)
        {
            _connectionManager.AddConnection(Context.ConnectionId, user);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectionManager.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
