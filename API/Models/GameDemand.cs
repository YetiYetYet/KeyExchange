using API.Db.Entity.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Models;

public class GameDemand : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set;}
    public Guid? CreatedBy { get; set; }
    public User? User { get; set; }
    public string? ContactName { get; set; }
    public string? ContactInfo { get; set; }
    public bool Approuved { get; set; } 
    public DateTime? CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}

public class GameDemandEntityConfiguration : Db.Entity.BaseEntityConfiguration<GameDemand>
{
    public override void Configure(EntityTypeBuilder<GameDemand> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Approuved).HasDefaultValueSql("0");
    }
}