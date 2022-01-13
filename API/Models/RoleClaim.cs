using API.Db.Entity.Entity.Interface;

namespace API.Models;

public class RoleClaim : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public Role? Role { get; set; }
    public Guid? CreatedBy { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}