﻿using API.Identity.Const;
using API.Identity.Dto;
using API.Service.User;
using API.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity;


[ApiController]
[Route("[controller]")]
public class RoleClaimsController : ControllerBase
{
    private readonly IRoleClaimsService _roleClaimService;

    public RoleClaimsController(IRoleClaimsService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }

    [Authorize(Policy = PermissionConstants.RoleClaims.View)]
    [HttpGet]
    public async Task<ActionResult<Result<List<RoleClaimResponse>>>> GetAllAsync()
    {
        var roleClaims = await _roleClaimService.GetAllAsync();
        return Ok(roleClaims);
    }

    [Authorize(Policy = PermissionConstants.RoleClaims.View)]
    [HttpGet("{roleId}")]
    public async Task<ActionResult<Result<List<RoleClaimResponse>>>> GetAllByRoleIdAsync([FromRoute] int roleId)
    {
        var response = await _roleClaimService.GetAllByRoleIdAsync(roleId);
        return Ok(response);
    }

    [Authorize(Policy = PermissionConstants.RoleClaims.Create)]
    [HttpPost]
    public async Task<ActionResult<Result<string>>> PostAsync(RoleClaimRequest request)
    {
        var response = await _roleClaimService.SaveAsync(request);
        return Ok(response);
    }

    [Authorize(Policy = PermissionConstants.RoleClaims.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Result<string>>> DeleteAsync(int id)
    {
        var response = await _roleClaimService.DeleteAsync(id);
        return Ok(response);
    }
}
