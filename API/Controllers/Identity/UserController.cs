using API.Identity.Dto;
using API.Service.User;
using API.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<Result<List<UserDetailsDto>>>> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Result<UserDetailsDto>>> GetByIdAsync(int id)
    {
        var user = await _userService.GetAsync(id);
        return Ok(user);
    }

    [HttpGet("{id:int}/roles")]
    public async Task<ActionResult<Result<UserRolesResponse>>> GetRolesAsync(int id)
    {
        var userRoles = await _userService.GetRolesAsync(id);
        return Ok(userRoles);
    }

    [HttpGet("{id:int}/permissions")]
    public async Task<ActionResult<Result<List<PermissionDto>>>> GetPermissionsAsync(int id)
    {
        var userPermissions = await _userService.GetPermissionsAsync(id);
        return Ok(userPermissions);
    }

    [HttpPost("{id:int}/roles")]
    public async Task<ActionResult<Result<string>>> AssignRolesAsync(int id, UserRolesRequest request)
    {
        var result = await _userService.AssignRolesAsync(id, request);
        return Ok(result);
    }

    [HttpPost("toggle-status")]
    public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        return Ok(await _userService.ToggleUserStatusAsync(request));
    }
}