using OscarChatPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscarChatPlatform.Application
{
    public interface INotificationService
    {
        Task SendChatIdToUsers(IReadOnlyList<string> connectionIds, string chatId);
        Task SendMessageToUsers(IReadOnlyList<string> connectionIds, string message, string senderId);
        Task TerminateChat(IReadOnlyList<string> connectionIds, string terminatedByUserId);
    }
}
