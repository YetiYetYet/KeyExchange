using API.Db;
using API.Wrapper;
using API.Identity.Const;
using API.Identity.Dto;
using API.Identity.Models;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using IResult = API.Wrapper.IResult;

namespace API.Service.User;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IStringLocalizer<UserService> _localizer;
    private readonly ApplicationDbContext _context;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, IStringLocalizer<UserService> localizer)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _localizer = localizer;
    }

    public async Task<IResult> SearchAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<UserDetailsDto>>> GetAllAsync()
    {
        var users = await _userManager.Users.AsNoTracking().ToListAsync();
        var userDetailsDtos = AutoMapperUtils.BasicAutoMapper<ApplicationUser, UserDetailsDto>(users);
        var hidedUsersDetails = userDetailsDtos.Select(HideInfo).ToList();
        return await Result<List<UserDetailsDto>>.SuccessAsync(hidedUsersDetails);
    }

    public async Task<int> GetCountAsync()
    {
        return await _userManager.Users.AsNoTracking().CountAsync();
    }

    public async Task<IResult<UserDetailsDto>> GetAsync(int userId)
    {
        Console.WriteLine("GetAsync" + userId );
        var user = await _userManager.Users.AsNoTracking().Where(u => u.Id == userId).FirstOrDefaultAsync();
        if (user is null)
        {
            return await Result<UserDetailsDto>.FailAsync(_localizer["ApplicationUser Not Found."]);
        }

        var userDetailsDtos = AutoMapperUtils.BasicAutoMapper<ApplicationUser, UserDetailsDto>(user);
        var hidedUserDetails = HideInfo(userDetailsDtos);
        return await Result<UserDetailsDto>.SuccessAsync(hidedUserDetails);
    }

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(int userId)
    {
        var viewModel = new List<UserRoleDto>();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
        foreach (var role in roles)
        {
            var userRolesViewModel = new UserRoleDto
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description
            };
            userRolesViewModel.Enabled = await _userManager.IsInRoleAsync(user, role.Name);

            viewModel.Add(userRolesViewModel);
        }

        var result = new UserRolesResponse { UserRoles = viewModel };
        return await Result<UserRolesResponse>.SuccessAsync(result);
    }

    public async Task<IResult<int>> AssignRolesAsync(int userId, UserRolesRequest? request)
    {
        ApplicationUser? user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        if (user == null)
        {
            return await Result<int>.FailAsync(_localizer["ApplicationUser Not Found."]);
        }

        if (request == null)
        {
            if (request?.UserRoles != null)
            {
                return await Result<int>.FailAsync(_localizer["Invalid Request."]);
            }
        }

        UserRoleDto? adminRole = request?.UserRoles!.Find(a => !a.Enabled && a.RoleName == RoleConstants.Admin);
        if (adminRole != null)
        {
            request?.UserRoles!.Remove(adminRole);
        }
        
        foreach (UserRoleDto userRole in request?.UserRoles!)
        {
            // Check if ApplicationRole Exists
            if (await _roleManager.FindByNameAsync(userRole.RoleName) == null) continue;
            if (userRole.Enabled)
            {
                if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, userRole.RoleName);
                }
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
            }
        }

        return await Result<int>.SuccessAsync(userId, string.Format(_localizer["ApplicationUser Roles Updated Successfully."]));
    }

    public async Task<Result<List<PermissionDto>>> GetPermissionsAsync(int id)
    {
        var userPermissions = new List<PermissionDto>();
        ApplicationUser? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return await Result<List<PermissionDto>>.FailAsync(_localizer["ApplicationUser Not Found."]);
        }

        var roleNames = await _userManager.GetRolesAsync(user);
        foreach (var role in _roleManager.Roles.Where(r => roleNames.Contains(r.Name)).ToList())
        {
            var permissions = await _context.RoleClaims.Where(a => a.RoleId == role.Id).ToListAsync();
            var permissionResponse = AutoMapperUtils.BasicAutoMapper<ApplicationRoleClaim ,PermissionDto>(permissions);
            userPermissions.AddRange(permissionResponse);
        }

        return await Result<List<PermissionDto>>.SuccessAsync(userPermissions.Distinct().ToList());
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
        if (user == null) return await Result<List<PermissionDto>>.FailAsync(_localizer["ApplicationUser Not Found."]);
        bool isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.Admin);
        bool isRoot = await _userManager.IsInRoleAsync(user, RoleConstants.Root);
        if (isAdmin || isRoot)
        {
            return await Result.FailAsync(_localizer["Administrators Profile's Status cannot be toggled"]);
        }
        
        user.IsActive = request.ActivateUser;
        IdentityResult? identityResult = await _userManager.UpdateAsync(user);
        return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(identityResult.Errors.ToString()!);

    }
    
    private UserDetailsDto HideInfo(UserDetailsDto userProfileDto)
    {
        UserDetailsDto hidedUserProfil = userProfileDto.ShallowCopy();
        if (!userProfileDto.ShowDiscord)
        {
            hidedUserProfil.Discord = null;
        }
        if (!userProfileDto.ShowEmail)
        {
            hidedUserProfil.Email = null;
        }
        if (!userProfileDto.ShowFirstName)
        {
            hidedUserProfil.FirstName = null;
        }
        if (!userProfileDto.ShowLastName)
        {
            hidedUserProfil.LastName = null;
        }
        if (!userProfileDto.ShowPhoneNumber)
        {
            hidedUserProfil.PhoneNumber = null;
        }
        return hidedUserProfil;
    }
}