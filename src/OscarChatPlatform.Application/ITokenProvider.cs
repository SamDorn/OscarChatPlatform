using OscarChatPlatform.Domain.Entities;

namespace OscarChatPlatform.Application
{
    public interface ITokenProvider
    {
        string GenerateToken(ApplicationUser user);
    }
}
