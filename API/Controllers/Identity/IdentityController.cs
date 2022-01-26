﻿using System.Security.Claims;
using API.Identity.Dto;
using API.Models;
using API.Service.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace API.Controllers.Identity;

[ApiController]
[Route("[controller]")]
public sealed class IdentityController : ControllerBase
{
    private readonly ICurrentUser _user;
    private readonly IIdentityService _identityService;
    private readonly IUserService _userService;

    public IdentityController(IIdentityService identityService, ICurrentUser user, IUserService userService)
    {
        _identityService = identityService;
        _user = user;
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<int>> RegisterAsync(RegisterUserRequest request)
    {
        var origin = GenerateOrigin();
        return Ok(await _identityService.RegisterAsync(request, origin));
    }


    // [HttpGet("confirm-email")]
    // [AllowAnonymous]
    // [ProducesResponseType(200)]
    // [ProducesDefaultResponseType(typeof(ErrorResult))]
    // public async Task<ActionResult<Result<string>>> ConfirmEmailAsync([FromQuery] string userId,
    //     [FromQuery] string code, [FromQuery] string tenant)
    // {
    //     return Ok(await _identityService.ConfirmEmailAsync(userId, code, tenant));
    // }

    // [HttpGet("confirm-phone-number")]
    // [AllowAnonymous]
    // [ProducesResponseType(200)]
    // [ProducesDefaultResponseType(typeof(ErrorResult))]
    // public async Task<ActionResult<Result<string>>> ConfirmPhoneNumberAsync([FromQuery] string userId,
    //     [FromQuery] string code)
    // {
    //     return Ok(await _identityService.ConfirmPhoneNumberAsync(userId, code));
    // }

    // [HttpPost("forgot-password")]
    // [AllowAnonymous]
    // [SwaggerHeader(HeaderConstants.Tenant, "Input your tenant Id to access this API", "", true)]
    // [ProducesResponseType(200)]
    // [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
    // [ProducesDefaultResponseType(typeof(ErrorResult))]
    // public async Task<ActionResult<Result>> ForgotPasswordAsync(ForgotPasswordRequest request)
    // {
    //     string origin = GenerateOrigin();
    //     return Ok(await _identityService.ForgotPasswordAsync(request, origin));
    // }

    // [HttpPost("reset-password")]
    // [AllowAnonymous]
    // [ProducesResponseType(200)]
    // [ProducesDefaultResponseType(typeof(ErrorResult))]
    // public async Task<ActionResult<Result>> ResetPasswordAsync(ResetPasswordRequest request)
    // {
    //     return Ok(await _identityService.ResetPasswordAsync(request));
    // }

    [HttpPut("profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> UpdateProfileAsync(UpdateProfileRequest request)
    {
        return Ok(await _identityService.UpdateProfileAsync(request, _user.GetUserId()));
    }

    [HttpGet("profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> GetProfileDetailsAsync()
    {
        var id = _user.GetUserId();
        var result = await _userService.GetAsync(id);
        return Ok(result);
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        IResult response = await _identityService.ChangePasswordAsync(model, _user.GetUserId());
        return Ok(response);
    }

    private string GenerateOrigin()
    {
        return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }
}