using API.Identity.Dto;
using API.Wrapper;
using IResult = API.Wrapper.IResult;

namespace API.Service.User;

public interface IUserService
{
    Task<IResult> SearchAsync();

    Task<Result<List<UserDetailsDto>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<IResult<UserDetailsDto>> GetAsync(int userId);

    Task<IResult<UserRolesResponse>> GetRolesAsync(int userId);

    Task<IResult<int>> AssignRolesAsync(int userId, UserRolesRequest? request);

    Task<Result<List<PermissionDto>>> GetPermissionsAsync(int id);

    Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);
}