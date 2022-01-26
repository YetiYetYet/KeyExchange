using System.Linq;
using API.Db;
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

public class RoleClaimsService : IRoleClaimsService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer<RoleClaimsService> _localizer;

    public RoleClaimsService(ApplicationDbContext context, IStringLocalizer<RoleClaimsService> localizer)
    {
        _db = context;
        _localizer = localizer;
    }

    public async Task<bool> HasPermissionAsync(int userId, string permission)
    {
        var userRoles = await _db.UserRoles.Where(a => a.UserId == userId).Select(a => a.RoleId).ToListAsync();
        var applicationRoles = await _db.Roles.Where(a => userRoles.Contains(a.Id)).ToListAsync();
        var roles = AutoMapperUtils.BasicAutoMapper<ApplicationRole, RoleDto>(applicationRoles);

        if (roles.Count != 0)
        {
            foreach (var role in roles)
            {
                if (_db.RoleClaims.Any(a => a.ClaimType == ClaimConstants.Permission && a.ClaimValue == permission && a.RoleId == role.Id))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllAsync()
    {
        var roleClaims = await _db.RoleClaims.ToListAsync();
        var roleClaimsResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRoleClaim, RoleClaimResponse>(roleClaims);
        return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
    }

    public Task<int> GetCountAsync() =>
        _db.RoleClaims.CountAsync();

    public async Task<Result<RoleClaimResponse>> GetByIdAsync(int id)
    {
        var roleClaim = await _db.RoleClaims
            .SingleOrDefaultAsync(x => x.Id == id);
        if (roleClaim is null)
        {
            return await Result<RoleClaimResponse>.FailAsync(_localizer["ApplicationRole not found."]);
        }

        var roleClaimResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRoleClaim, RoleClaimResponse>(roleClaim);
        return await Result<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(int roleId)
    {
        var roleClaims = await _db.RoleClaims
            .Where(x => x.RoleId == roleId)
            .ToListAsync();
        var roleClaimsResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRoleClaim, RoleClaimResponse>(roleClaims);
        return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
    }

    public async Task<Result<string>> SaveAsync(RoleClaimRequest request)
    {
        if (request.RoleId == null)
        {
            return await Result<string>.FailAsync(_localizer["ApplicationRole is required."]);
        }

        if (request.Id == 0)
        {
            var existingRoleClaim =
                await _db.RoleClaims
                    .SingleOrDefaultAsync(x =>
                        x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
            if (existingRoleClaim is not null)
            {
                return await Result<string>.FailAsync(_localizer["Similar ApplicationRole Claim already exists."]);
            }

            var roleClaim = AutoMapperUtils.BasicAutoMapper<RoleClaimRequest, ApplicationRoleClaim>(request);
            await _db.RoleClaims.AddAsync(roleClaim);
            await _db.SaveChangesAsync();
            return await Result<string>.SuccessAsync(string.Format(_localizer["ApplicationRole Claim {0} created."], request.Value));
        }
        else
        {
            var existingRoleClaim =
                await _db.RoleClaims
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
            if (existingRoleClaim is null)
            {
                return await Result<string>.SuccessAsync(_localizer["ApplicationRole Claim does not exist."]);
            }
            else
            {
                existingRoleClaim.ClaimType = request.Type;
                existingRoleClaim.ClaimValue = request.Value;
                existingRoleClaim.Group = request.Group;
                existingRoleClaim.Description = request.Description;
                existingRoleClaim.RoleId = (int)request.RoleId;
                _db.RoleClaims.Update(existingRoleClaim);
                await _db.SaveChangesAsync();
                return await Result<string>.SuccessAsync(string.Format(_localizer["ApplicationRole Claim {0} for ApplicationRole updated."], request.Value));
            }
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        var existingRoleClaim = await _db.RoleClaims
            .FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoleClaim is not null)
        {
            _db.RoleClaims.Remove(existingRoleClaim);
            await _db.SaveChangesAsync();
            return await Result<string>.SuccessAsync(string.Format(_localizer["ApplicationRole Claim {0} for ApplicationRole deleted."], existingRoleClaim.ClaimValue));
        }
        else
        {
            return await Result<string>.FailAsync(_localizer["ApplicationRole Claim does not exist."]);
        }
    }
}