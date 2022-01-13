using API.Db.Entity.Entity.Interface;

namespace API.Models;

public class Game : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public ApplicationUser? User { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool IsAvailable { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
    public string? Platforme { get; set; }
    public GameInfoFromPlatform? GameInfoFromPlatform { get; set; }
    public string? Key { get; set; }
    public string? ObtenaidBy { get; set; }
    public string? PublicComment { get; set; }
    public string? OwnerComment { get; set; }
    public string? AdminComment { get; set; }
    public List<GameDemand>? GameDemands { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime? GivenDate { get; set; }
    public string? GivenTo { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}