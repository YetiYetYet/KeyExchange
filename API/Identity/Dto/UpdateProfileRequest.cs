namespace API.Identity.Dto;

public class UpdateProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Discord { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? ShowPhoneNumber { get; set; } = false;
    public bool? ShowEmail { get; set; } = false;
    public bool? ShowDiscord { get; set; } = false;
    public bool? ShowFirstName { get; set; } = false;
    public bool? ShowLastName { get; set; } = false;
    public string? PictureUri { get; set; }
    public string? Description { get; set; }
    public string? OtherLink { get; set; }
}