using System.Security.Claims;
using API.DTO.Mailing;
using API.Exception;
using API.Identity.Const;
using API.Identity.Dto;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
namespace API.Service.User;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IStringLocalizer<IdentityService> _localizer;

    public IdentityService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IStringLocalizer<IdentityService> localizer)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _localizer = localizer;
    }

    public async Task<IResult> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? objectId = principal.GetUserId();
        if (string.IsNullOrWhiteSpace(objectId))
        {
            throw new IdentityExecption.IdentityException(_localizer["Invalid objectId"]);
        }
        var id = int.Parse(objectId);
        ApplicationUser? user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync() ?? await CreateOrUpdateFromPrincipalAsync(principal);

        // Add user to incoming role if that isn't the case yet
        if (principal.FindFirstValue(ClaimTypes.Role) is string role &&
            await _roleManager.RoleExistsAsync(role) &&
            !await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return Results.Ok(user.Id);
    }
    
    private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? email = principal.FindFirstValue(ClaimTypes.Email);
        string? username = principal.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            throw new IdentityExecption.IdentityException(string.Format(_localizer["Username or Email not valid."]));
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is not null)
        {
            throw new IdentityExecption.IdentityException(string.Format(_localizer["Username {0} is already taken."], username));
        }

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                throw new IdentityExecption.IdentityException(string.Format(_localizer["Email {0} is already taken."], email));
            }
        }
        

        IdentityResult? result;
        if (user is not null)
        {
            result = await _userManager.UpdateAsync(user);
        }
        else
        {
            user = new ApplicationUser
            {
                FirstName = principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = principal.FindFirstValue(ClaimTypes.Surname),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
            };
            result = await _userManager.CreateAsync(user);
        }

        if (!result.Succeeded)
        {
            throw new IdentityExecption.IdentityException(_localizer["Validation Errors Occurred."], result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
        }

        return user;
    }

    public async Task<IResult> RegisterAsync(RegisterUserRequest request, string origin)
    {
        ApplicationUser? userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
        if (userWithSameUserName != null)
        {
            throw new IdentityExecption.IdentityException(string.Format(_localizer["Username {0} is already taken."], request.Username));
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Username,
            PhoneNumber = request.PhoneNumber,
            Discord = request.Discord,
            IsActive = true,
        };
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            ApplicationUser? userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                throw new IdentityExecption.IdentityException(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
            }
        }

        if (await _userManager.FindByEmailAsync(request.Email?.Normalize()) is not null)
        {
            throw new IdentityExecption.IdentityException(string.Format(_localizer["Email {0} is already registered."], request.Email));
        }

        IdentityResult? result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new IdentityExecption.IdentityException(_localizer["Validation Errors Occurred."], result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
        }

        await _userManager.AddToRoleAsync(user, RoleConstants.Basic);

        var messages = new List<string> { $"ApplicationUser {user.UserName} Registered." };

        // if (_mailSettings.EnableVerification && !string.IsNullOrEmpty(user.Email))
        // {
        //     // send verification email
        //     string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
        //     var mailRequest = new MailRequest(
        //         new List<string> { user.Email },
        //         _localizer["Confirm Registration"],
        //         _templateService.GenerateEmailConfirmationMail(user.UserName ?? "ApplicationUser", user.Email, emailVerificationUri));
        //     _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));
        //     messages.Add(_localizer[$"Please check {user.Email} to verify your account!"]);
        // }

        return Results.Ok(user.Id);
    }

    public async Task<IResult> ConfirmEmailAsync(int userId, string code)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> ConfirmPhoneNumberAsync(int userId, string code)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, int userId)
    {
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber && x.Id != userId);
            if (userWithSamePhoneNumber != null)
            {
                return Results.Conflict($"Phone number {request.PhoneNumber} is already registered.");
            }
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email?.Normalize());
            if (userWithSameEmail != null && userWithSameEmail.Id != userId)
                return Results.Conflict($"This Email : {request.Email} is already registered.");
        }
        
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Results.NotFound($"ApplicationUser with id {userId} Not Found.");
        }

        user.PictureUri = request.PictureUri ?? user.PictureUri;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.Description = request.Description ?? user.Description;
        user.Discord = request.Discord ?? user.Discord;
        user.OtherLink = request.OtherLink ?? user.OtherLink;
        user.ShowDiscord = request.ShowDiscord ?? user.ShowDiscord;
        user.ShowEmail = request.ShowEmail ?? user.ShowEmail;
        user.ShowFirstName = request.ShowFirstName ?? user.ShowFirstName;
        user.ShowLastName = request.ShowLastName ?? user.ShowLastName;
        
        string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        var identityResult = await _userManager.UpdateAsync(user);
        var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
        //await _signInManager.RefreshSignInAsync(user);
        return identityResult.Succeeded ? Results.Ok() : Results.Problem(identityResult.Errors.ToString());
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, int userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Results.NotFound(userId);
        }

        var identityResult = await _userManager.ChangePasswordAsync(
            user,
            request.Password,
            request.NewPassword);
        return identityResult.Succeeded ? Results.Ok() : Results.Problem(identityResult.Errors.ToString());
    }
}