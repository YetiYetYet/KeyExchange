using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Utils;
using Microsoft.AspNetCore.Authentication;
using NuGet.Common;

namespace API.Service.User;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    public IHttpContextAccessor HttpContextAccessor { get; set; }

    private ClaimsPrincipal _user;

    private bool _isAuthentificated = false;

    public string? Name => _user?.Identity?.Name;

    private int _userId = 0;
    
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
        
    }
    
    public int GetUserId()
    {
        return _isAuthentificated ? _userId : 0;
    }

    public string? GetUserEmail()
    {
        var email = _isAuthentificated ? _user?.GetUserEmail() : string.Empty;
        return email;
    }

    public bool IsAuthenticated()
    {
        var isAuth = _user?.Identity?.IsAuthenticated ?? false;
        return isAuth;
        var claim = HttpContextAccessor.HttpContext.User.Claims;
        return true;
    }

    public bool IsInRole(string role)
    {
        return _user?.IsInRole(role) ?? false;
    }

    public IEnumerable<Claim>? GetUserClaims()
    {
        var claims = _user?.Claims;
        //Console.WriteLine($"GetUserClaims() : {claims}");
        return claims;
    }

    public void SetUser(HttpContext context)
    {
        if (_user != null)
        {
            throw new System.Exception("Method reserved for in-scope initialization");
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null) return;
        _isAuthentificated = new JwtSecurityTokenHandler().CanValidateToken;
        JwtSecurityToken? jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Console.WriteLine(token);
        _user = new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken.Claims));
        var id = _user.FindFirstValue(ClaimTypes.NameIdentifier);
        _userId = !string.IsNullOrEmpty(id) ? int.Parse(id) : 0;
    }
    
    

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new System.Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(int userId)
    {
        if (_userId != 0)
        {
            throw new System.Exception("Method reserved for in-scope initialization");
        }

        _userId = userId;
    }
}