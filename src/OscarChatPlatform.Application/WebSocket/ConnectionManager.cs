using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatRoom.Application.WebSocket
{

    public interface IConnectionManager
    {
        void AddConnection(string connectionId, UserChatHubDTO user);
        void RemoveConnection(string connectionId);
        IEnumerable<string> GetConnectionIds(string anonymousUserId);
    }

    public class ConnectionManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<string, UserChatHubDTO> _connections = new();

        public void AddConnection(string connectionId, UserChatHubDTO user)
        {

            _connections[connectionId] = user;
        }

        public void RemoveConnection(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }

        public IEnumerable<string> GetConnectionIds(string anonymousUserId)
        {
            //return _connections.Select(c => c.Value.Select(user => user.Key == anonymousUserId));


            return [];
        }
    }
    public class UserChatHubDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public UserChatHubDTO(string userId, string username)
        {
            UserId = userId;
            Username = username;
        }
    }
}
