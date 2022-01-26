using System.Security.Claims;

namespace API.Service.User;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(int userId);
}