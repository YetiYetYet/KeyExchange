namespace API.Identity.Dto;

public record TokenResponse(string Token, DateTime RefreshTokenExpiryTime);