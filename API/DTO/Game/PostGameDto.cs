namespace API.DTO.Game;

public class PostGameDto
{
    public bool IsAvailable { get; set; } = true;
    public string? Name { get; set; }
    public string? Link { get; set; }
    public string? Platforme { get; set; } = "Steam";
    public string? Key { get; set; }
    public string? ObtenaidBy { get; set; }
    public string? PublicComment { get; set; }
    public string? AdminComment { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime? GivenDate { get; set; } = null;
    public string? GivenTo { get; set; } = null;
}