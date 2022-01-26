﻿namespace API.Identity.Dto;

public class UserDetailsDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public bool ShowPhoneNumber { get; set; }
    public bool ShowEmail { get; set; }
    public bool ShowDiscord { get; set; }
    public bool ShowFirstName { get; set; }
    public bool ShowLastName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Discord { get; set; }
    public string? Email { get; set; }
    public string? PictureUri { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Description { get; set; }
    public string? OtherLink { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? CreatedOn { get; set; }
    
    public UserDetailsDto ShallowCopy()
    {
        return (UserDetailsDto)MemberwiseClone();
    }
    
}
