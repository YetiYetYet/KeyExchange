using API.Identity.Dto;
using API.Service.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity;

[ApiController]
[Route("[controller]")]
public sealed class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Result<TokenResponse>>> GetTokenAsync(TokenRequest request)
    {
        var token = await _tokenService.GetTokenAsync(request, GenerateIpAddress());
        
        return Ok(token);
    }

    // [HttpPost("refresh")]
    // [AllowAnonymous]
    // public async Task<ActionResult<Result<TokenResponse>>> RefreshAsync(RefreshTokenRequest request)
    // {
    //     var response = await _tokenService.RefreshTokenAsync(request, GenerateIpAddress());
    //     return Ok(response);
    // }

    private string GenerateIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
        }
    }
}