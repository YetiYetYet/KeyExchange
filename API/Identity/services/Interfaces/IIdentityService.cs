using System.Security.Claims;
using API.Identity.Dto;



namespace API.Service.User;

public interface IIdentityService
{
    Task<IResult> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);

    Task<IResult> RegisterAsync(RegisterUserRequest request, string origin);

    Task<IResult> ConfirmEmailAsync(int userId, string code);

    Task<IResult> ConfirmPhoneNumberAsync(int userId, string code);

    Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request);

    Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);

    Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, int userId);
    
    Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, int userId);
}