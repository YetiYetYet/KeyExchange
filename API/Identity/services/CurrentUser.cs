using System.Security.Claims;

namespace API.Service.User;

public class CurrentUser : ICurrentUser
{
    public IHttpContextAccessor HttpContextAccessor { get; set; }

    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private int _userId = 0;
    
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }
    
    public int GetUserId() =>
        IsAuthenticated() ?  0 : _userId;

    public string? GetUserEmail() =>
        IsAuthenticated() ? _user?.GetUserEmail() : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) ?? false;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public void SetUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }
        _user = user;
    }

    public void SetUserJob(int? userId)
    {
        if (_userId != 0)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (userId != null)
        {
            _userId = (int)userId;
        }
    }
}