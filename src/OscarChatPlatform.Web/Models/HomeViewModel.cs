﻿namespace OscarChatPlatform.Web.Models
{
    public class HomeViewModel
    {
        /// <summary>
        /// User Identifier of the current user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Indicates if the user never landed on the homepage
        /// </summary>
        public bool IsNewUser { get; set; }

        /// <summary>
        /// Number of active chats on the platform
        /// </summary>
        public int ActiveChats { get; set; } = 0;

        /// <summary>
        /// Number of active users on the platform
        /// </summary>
        public int OnlineUsers { get; set; } = 0;
    }
}
