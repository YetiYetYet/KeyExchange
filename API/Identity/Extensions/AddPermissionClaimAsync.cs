using System.Security.Claims;
using API.Identity.Const;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Service.User;

public static class ClaimsExtension
{
    public static async Task<IdentityResult> AddPermissionClaimAsync(this RoleManager<ApplicationRole> roleManager, ApplicationRole applicationRole, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(applicationRole);
        if (!allClaims.Any(a => a.Type == ClaimConstants.Permission && a.Value == permission))
        {
            return await roleManager.AddClaimAsync(applicationRole, new Claim(ClaimConstants.Permission, permission));
        }

        return IdentityResult.Failed();
    }
}