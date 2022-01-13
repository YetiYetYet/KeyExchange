using API.Db.Entity.Entity.Interface;

namespace API.Models;

public class GameInfoFromPlatform : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Reviews { get; set; }
    public string? TumbnailUrl { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}

