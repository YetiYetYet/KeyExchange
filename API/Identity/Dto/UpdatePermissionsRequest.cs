namespace API.Identity.Dto;

public class UpdatePermissionsRequest
{
    public string? Permission { get; set; }
    public bool Enabled { get; set; }
}