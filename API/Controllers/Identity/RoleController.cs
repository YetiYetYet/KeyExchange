using API.Identity;
using API.Identity.Const;
using API.Identity.Dto;
using API.Service.User;
using API.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("all")]
    [MustHavePermission(PermissionConstants.Roles.ListAll)]
    public async Task<ActionResult<Result<List<RoleDto>>>> GetListAsync()
    {
        var roles = await _roleService.GetListAsync();
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    [MustHavePermission(PermissionConstants.Roles.View)]
    public async Task<ActionResult<Result<RoleDto>>> GetByIdAsync(int id)
    {
        var roles = await _roleService.GetByIdAsync(id);
        return Ok(roles);
    }

    [HttpGet("{id:int}/permissions")]
    public async Task<ActionResult<Result<List<PermissionDto>>>> GetPermissionsAsync(int id)
    {
        var roles = await _roleService.GetPermissionsAsync(id);
        return Ok(roles);
    }

    [HttpPut("{id:int}/permissions")]
    public async Task<ActionResult<Result<string>>> UpdatePermissionsAsync(int id,
        List<UpdatePermissionsRequest> request)
    {
        var roles = await _roleService.UpdatePermissionsAsync(id, request);
        return Ok(roles);
    }

    [HttpPost]
    [MustHavePermission(PermissionConstants.Roles.Register)]
    public async Task<ActionResult<Result<string>>> RegisterRoleAsync(RoleRequest request)
    {
        var response = await _roleService.RegisterRoleAsync(request);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [MustHavePermission(PermissionConstants.Roles.Remove)]
    public async Task<ActionResult<Result<string>>> DeleteAsync(int id)
    {
        var response = await _roleService.DeleteAsync(id);
        return Ok(response);
    }
}
