using API.Db.Entity.Entity.Interface;

namespace API.Models;

public class GameDemand : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set;}
    public Guid? CreatedBy { get; set; }
    public Game Game { get; set; }
    public ApplicationUser? User { get; set; }
    public string ContactInfo { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}