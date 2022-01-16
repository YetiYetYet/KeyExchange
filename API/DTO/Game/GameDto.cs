namespace API.DTO.Game;

public class GameDto
{
    public Guid UserId { get; set; }
    public UserGameDto User { get; set; }
    public bool IsAvailable { get; set; }
    public string Name { get; set; }
    public string Platforme { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Reviews { get; set; }
    public string? TumbnailUrl { get; set; }
    public string PublicComment { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime GivenDate { get; set; }
    public string GivenTo { get; set; }
}