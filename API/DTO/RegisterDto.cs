using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class RegisterDto
{
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Discord { get; set; } = null;
    public string? PhoneNumber { get; set; } = null;
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}