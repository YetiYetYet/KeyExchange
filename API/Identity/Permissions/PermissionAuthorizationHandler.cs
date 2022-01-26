using API.Service.User;
using Microsoft.AspNetCore.Authorization;

namespace API.Identity;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IRoleClaimsService _permissionService;

    public PermissionAuthorizationHandler(IRoleClaimsService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User?.GetUserId();
        if (userId is not null &&
            await _permissionService.HasPermissionAsync(int.Parse(userId), requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}