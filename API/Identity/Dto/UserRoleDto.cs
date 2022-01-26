namespace API.Identity.Dto;

public class UserRoleDto
{
    public int? Id { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public bool Enabled { get; set; }
}