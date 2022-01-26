

using System.ComponentModel.DataAnnotations;

namespace API.Identity.Dto;

public class UserLoginRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}