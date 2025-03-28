using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OscarChatPlatform.Web.Extensions
{
    public class CookieJwtBearerEvents : JwtBearerEvents
    {
        private readonly string _cookieName;

        public CookieJwtBearerEvents(string cookieName)
        {
            _cookieName = cookieName;
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            // Read the token from the cookie instead of the Authorization header
            if (context.Request.Cookies.TryGetValue(_cookieName, out var token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    }
}
