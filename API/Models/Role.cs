using API.Db.Entity.Entity.Interface;

namespace API.Models;

public class Role : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ValidUpto { get; set; }
    public string? Description { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}