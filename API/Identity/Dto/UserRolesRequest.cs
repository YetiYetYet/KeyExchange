namespace API.Identity.Dto;

public class UserRolesRequest
{
    public List<UserRoleDto> UserRoles { get; set; } = new();
}