using System.ComponentModel.DataAnnotations;

namespace API.DTO.Authentification;

public class RegisterDto
{
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Discord { get; set; } = null;
    public string? PhoneNumber { get; set; } = null;
    [Required]
    public string Email { get; set; }
    [Required]
    public string ConfirmEmail { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}