using System.Security.Claims;

namespace API.Service.User;

public interface ICurrentUser
{
    IHttpContextAccessor HttpContextAccessor { get; set; }
    
    string? Name { get; }

    int GetUserId();

    string? GetUserEmail();
    
    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();

    void SetUser(ClaimsPrincipal user);
}