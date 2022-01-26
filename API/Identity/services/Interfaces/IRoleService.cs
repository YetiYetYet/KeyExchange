using API.Identity.Dto;
using API.Wrapper;

namespace API.Service.User;


public interface IRoleService
{
    Task<Result<List<RoleDto>>> GetListAsync();

    Task<Result<List<PermissionDto>>> GetPermissionsAsync(int id);

    Task<int> GetCountAsync();

    Task<Result<RoleDto>> GetByIdAsync(int id);

    Task<bool> ExistsAsync(string roleName, int? excludeId);

    Task<Result<string>> RegisterRoleAsync(RoleRequest request);

    Task<Result<string>> DeleteAsync(int id);

    Task<Result<List<RoleDto>>> GetUserRolesAsync(int userId);

    Task<Result<string>> UpdatePermissionsAsync(int id, List<UpdatePermissionsRequest> request);
}
