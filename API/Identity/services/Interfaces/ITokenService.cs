using API.Identity.Dto;
using API.Wrapper;

namespace API.Service.User;

public interface ITokenService
{
    Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);

    Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}