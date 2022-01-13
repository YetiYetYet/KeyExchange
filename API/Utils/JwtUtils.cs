using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Models;
using Microsoft.IdentityModel.Tokens;

namespace API.Utils;

public static class JwtUtils
{
    public static string CreateToken(User user, string secret)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.Key),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(secret));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}