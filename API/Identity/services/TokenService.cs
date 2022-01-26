using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Exception;
using API.Identity.Const;
using API.Identity.Dto;
using API.Models;
using API.Utils.Jwt;
using API.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Service.User;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IStringLocalizer<TokenService> _localizer;
    private readonly JwtSettings _jwtSettings;

    public TokenService(UserManager<ApplicationUser> userManager, IStringLocalizer<TokenService> localizer, IOptions<JwtSettings> jwtSettings, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _localizer = localizer;
        //_signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());
        if (user == null)
        {
            throw new IdentityExecption.IdentityException(_localizer["identity.usernotfound"], statusCode: HttpStatusCode.Unauthorized);
        }

        if (!user.IsActive)
        {
            throw new IdentityExecption.IdentityException(_localizer["identity.usernotactive"], statusCode: HttpStatusCode.Unauthorized);
        }

        // if (_mailSettings.EnableVerification && !user.EmailConfirmed)
        // {
        //     throw new IdentityExecption.IdentityException(_localizer["identity.emailnotconfirmed"], statusCode: HttpStatusCode.Unauthorized);
        // }
        bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        //var passwordValid = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (!passwordValid)
        {
            throw new IdentityExecption.IdentityException(_localizer["identity.invalidcredentials"], statusCode: HttpStatusCode.Unauthorized);
        }
        
        user.LastLogin = DateTime.Now;
        await _userManager.UpdateAsync(user);
        var token = GenerateJwt(user, ipAddress);
        DateTime refreshToken = DateTime.Now.AddDays(_jwtSettings.TokenExpirationInMinutes);
        TokenResponse response = new(token, refreshToken);
        return await Result<TokenResponse>.SuccessAsync(response);
    }

    public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
    {
        throw new IdentityExecption.IdentityException(_localizer["identity.NotImplemented"], statusCode: HttpStatusCode.Unauthorized);
        // if (request is null)
        // {
        //     throw new IdentityExecption.IdentityException(_localizer["identity.invalidtoken"], statusCode: HttpStatusCode.Unauthorized);
        // }
        //
        // ClaimsPrincipal userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        // var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        // ApplicationUser? user = await _userManager.FindByEmailAsync(userEmail);
        // if (user == null)
        // {
        //     throw new IdentityExecption.IdentityException(_localizer["identity.usernotfound"], statusCode: HttpStatusCode.NotFound);
        // }
        //
        // if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        // {
        //     throw new IdentityExecption.IdentityException(_localizer["identity.invalidrefreshtoken"], statusCode: HttpStatusCode.Unauthorized);
        // }
        //
        // string token = GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
        // user.RefreshToken = GenerateRefreshToken();
        // user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
        // await _userManager.UpdateAsync(user);
        // var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
        // return await Result<TokenResponse>.SuccessAsync(response);
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
    }

    private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress)
    {
        return new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimConstants.Fullname, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new(ClaimConstants.IpAddress, ipAddress),
                new(ClaimConstants.ImageUrl, user.PictureUri ?? string.Empty),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            };
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.Now.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new IdentityExecption.IdentityException(_localizer["identity.invalidtoken"], statusCode: HttpStatusCode.Unauthorized);
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}
