using API.Db.Entity.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    public bool SoftDeleted { get; set; } = false;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}

public class RoleGameEntityConfiguration : Db.Entity.BaseEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.IsActive).HasDefaultValueSql("1");
    }
}