using System.Linq;
using System.Net;
using API.Db;
using API.Exception;
using API.Identity.Const;
using API.Identity.Dto;
using API.Identity.Models;
using API.Models;
using API.Utils;
using API.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace API.Service.User;

public class RoleService : IRoleService
{
    private readonly ICurrentUser _currentUser;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<RoleService> _localizer;
    private readonly IRoleClaimsService _roleClaimService;

    public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IStringLocalizer<RoleService> localizer, ICurrentUser currentUser, IRoleClaimsService roleClaimService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
        _localizer = localizer;
        _currentUser = currentUser;
        _roleClaimService = roleClaimService;
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        var existingRole = await _roleManager.FindByIdAsync(id.ToString());
        if (existingRole == null)
        {
            throw new IdentityExecption.IdentityException("ApplicationRole Not Found", statusCode: HttpStatusCode.NotFound);
        }

        if (DefaultRoles.Contains(existingRole.Name))
        {
            return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete {0} ApplicationRole."], existingRole.Name));
        }

        bool roleIsNotUsed = true;
        var allUsers = await _userManager.Users.ToListAsync();
        foreach (var user in allUsers)
        {
            if (await _userManager.IsInRoleAsync(user, existingRole.Name))
            {
                roleIsNotUsed = false;
            }
        }

        if (roleIsNotUsed)
        {
            await _roleManager.DeleteAsync(existingRole);
            return await Result<string>.SuccessAsync(existingRole.Id.ToString(), string.Format(_localizer["ApplicationRole {0} Deleted."], existingRole.Name));
        }
        else
        {
            return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete {0} ApplicationRole as it is being used."], existingRole.Name));
        }
    }

    public async Task<Result<RoleDto>> GetByIdAsync(int id)
    {
        var role = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
        if (role is null)
        {
            throw new IdentityExecption.IdentityException("ApplicationRole Not Found", statusCode: HttpStatusCode.NotFound);
        }

        var rolesResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRole, RoleDto>(role);
        rolesResponse.IsDefault = DefaultRoles.Contains(role.Name);
        return await Result<RoleDto>.SuccessAsync(rolesResponse);
    }

    public async Task<int> GetCountAsync()
    {
        return await _roleManager.Roles.CountAsync();
    }

    public async Task<Result<List<RoleDto>>> GetListAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var rolesResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRole, RoleDto>(roles);
        foreach (var role in rolesResponse)
        {
            role.IsDefault = DefaultRoles.Contains(role.Name);
        }

        return await Result<List<RoleDto>>.SuccessAsync(rolesResponse);
    }

    public async Task<Result<List<PermissionDto>>> GetPermissionsAsync(int id)
    {
        var permissions = await _context.RoleClaims.Where(a => a.RoleId == id && a.ClaimType == ClaimConstants.Permission).ToListAsync();
        var permissionResponse =AutoMapperUtils.BasicAutoMapper<ApplicationRoleClaim, PermissionDto>(permissions);
        return await Result<List<PermissionDto>>.SuccessAsync(permissionResponse);
    }

    public async Task<Result<List<RoleDto>>> GetUserRolesAsync(int userId)
    {
        var userRoles = await _context.UserRoles.Where(a => a.UserId == userId).Select(a => a.RoleId).ToListAsync();
        var roles = await _roleManager.Roles.Where(a => userRoles.Contains(a.Id)).ToListAsync();

        var rolesResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRole, RoleDto>(roles);
        foreach (var role in rolesResponse)
        {
            role.IsDefault = DefaultRoles.Contains(role.Name);
        }

        return await Result<List<RoleDto>>.SuccessAsync(rolesResponse);
    }

    public async Task<bool> ExistsAsync(string roleName, int? excludeId) =>
        await _roleManager.FindByNameAsync(roleName)
            is ApplicationRole existingRole
            && existingRole.Id != excludeId;

    public async Task<Result<string>> RegisterRoleAsync(RoleRequest request)
    {
        if (request.Id != null)
        {
            var newRole = new ApplicationRole()
            {
                Name = request.Name,
                NormalizedName = request.Name.ToUpper(),
                Description = request.Description
            };
            var response = await _roleManager.CreateAsync(newRole);
            await _context.SaveChangesAsync();
            if (response.Succeeded)
            {
                return await Result<string>.SuccessAsync(newRole.Id.ToString(), string.Format(_localizer["ApplicationRole {0} Created."], request.Name));
            }
            return await Result<string>.FailAsync(response.Errors.Select(e => _localizer[e.Description].ToString()).ToList());
        }
        else
        {
            var existingRole = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (existingRole == null)
            {
                return await Result<string>.FailAsync(_localizer["ApplicationRole does not exist."]);
            }

            if (DefaultRoles.Contains(existingRole.Name))
            {
                return await Result<string>.SuccessAsync(string.Format(_localizer["Not allowed to modify {0} ApplicationRole."], existingRole.Name));
            }

            existingRole.Name = request.Name;
            existingRole.NormalizedName = request.Name.ToUpper();
            existingRole.Description = request.Description;
            var result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(existingRole.Id.ToString(), string.Format(_localizer["ApplicationRole {0} Updated."], existingRole.Name));
            }
            else
            {
                return await Result<string>.FailAsync(result.Errors.Select(e => _localizer[e.Description].ToString()).ToList());
            }
        }
    }

    public async Task<Result<string>> UpdatePermissionsAsync(int roleId, List<UpdatePermissionsRequest> request)
    {
        try
        {
            var errors = new List<string>();
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return await Result<string>.FailAsync(_localizer["ApplicationRole does not exist."]);
            }

            if (role.Name == RoleConstants.Admin)
            {
                var currentUser = await _userManager.Users.SingleAsync(x => x.Id == _currentUser.GetUserId());
                if (await _userManager.IsInRoleAsync(currentUser, RoleConstants.Admin))
                {
                    return await Result<string>.FailAsync(_localizer["Not allowed to modify Permissions for this ApplicationRole."]);
                }
            }

            var selectedPermissions = request.Where(a => a.Enabled).ToList();
            if (role.Name == RoleConstants.Admin)
            {
                if (!selectedPermissions.Any(x => x.Permission == PermissionConstants.Roles.View)
                   || !selectedPermissions.Any(x => x.Permission == PermissionConstants.RoleClaims.View)
                   || !selectedPermissions.Any(x => x.Permission == PermissionConstants.RoleClaims.Edit))
                {
                    return await Result<string>.FailAsync(string.Format(
                        _localizer["Not allowed to deselect {0} or {1} or {2} for this ApplicationRole."],
                        PermissionConstants.Roles.View,
                        PermissionConstants.RoleClaims.View,
                        PermissionConstants.RoleClaims.Edit));
                }
            }

            var permissions = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in permissions.Where(p => request.Any(a => a.Permission == p.Value)))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (var permission in selectedPermissions)
            {
                if (!string.IsNullOrEmpty(permission.Permission))
                {
                    var addResult = await _roleManager.AddPermissionClaimAsync(role, permission.Permission);
                    if (!addResult.Succeeded)
                    {
                        errors.AddRange(addResult.Errors.Select(e => _localizer[e.Description].ToString()));
                    }
                }
            }

            var addedPermissions = await _roleClaimService.GetAllByRoleIdAsync(role.Id);
            if (addedPermissions.Succeeded)
            {
                foreach (var permission in selectedPermissions)
                {
                    var addedPermission = addedPermissions.Data?.SingleOrDefault(x => x.Type == ClaimConstants.Permission && x.Value == permission.Permission);
                    if (addedPermission != null)
                    {
                        var newPermission =
                            AutoMapperUtils.BasicAutoMapper<RoleClaimResponse, RoleClaimRequest>(addedPermission);
                        var saveResult = await _roleClaimService.SaveAsync(newPermission);
                        if (!saveResult.Succeeded && saveResult.Messages is not null)
                        {
                            errors.AddRange(saveResult.Messages);
                        }
                    }
                }
            }
            else if (addedPermissions.Messages is not null)
            {
                errors.AddRange(addedPermissions.Messages);
            }

            if (errors.Count > 0)
            {
                return await Result<string>.FailAsync(errors);
            }

            return await Result<string>.SuccessAsync(_localizer["Permissions Updated."]);
        }
        catch (System.Exception ex)
        {
            return await Result<string>.FailAsync(ex.Message);
        }
    }

    internal static List<string> DefaultRoles =>
        typeof(RoleConstants).GetAllPublicConstantValues<string>();
}