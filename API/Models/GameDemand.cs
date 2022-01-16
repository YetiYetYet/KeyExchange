using System.ComponentModel.DataAnnotations.Schema;
using API.Db.Entity.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Models;

public class GameDemand : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set;}
    public Guid? CreatedBy { get; set; }
    [ForeignKey("user_id")]
    public Guid? UserId { get; set; } // Foreign key
    public User? User { get; set; } // Navigation parameter
    [ForeignKey("game_id")]
    public Guid? GameId { get; set; } // Foreign key
    public Game? Game { get; set; } // Navigation parameter
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
        builder.HasOne(gameDemand => gameDemand.User).WithMany(user => user.GameDemands).HasForeignKey(gameDemand => gameDemand.UserId);
        builder.HasOne(gameDemand => gameDemand.Game).WithMany(game => game.GameDemands).HasForeignKey(gameDemand => gameDemand.UserId);
    }
}