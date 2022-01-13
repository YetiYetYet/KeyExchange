using API.Models;

namespace API.DTO;

public class UserDto
{
    public string Username { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Discord { get; set; }
    public string? PhoneNumber { get; set; }
    public Role? Role { get; set; }
    public bool? IsPublic { get; set; }
    public bool? ShowPhoneNumber { get; set; }
    public bool? ShowEmail { get; set; }
    public bool? ShowDiscord { get; set; }
    public bool? ShowFirstName { get; set; }
    public bool? ShowLastName { get; set; }
    public string? PictureUri { get; set; }
    public string? OtherLink { get; set; }
    public bool IsActive { get; set; }
    public List<Game>? Games { get; set; }
    public DateTime? LastLogin { get; set; }
}