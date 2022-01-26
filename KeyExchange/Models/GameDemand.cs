using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyExchange.Database;
using KeyExchange.Database.Interfaces;
using KeyExchange.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyExchange.Models;

public class GameDemand : IBaseEntity, ISoftDelete
{
    [Key]
    public int Id { get; set;}
    public int? CreatedBy { get; set; }
    [ForeignKey("user_id")]
    public int? UserId { get; set; } // Foreign key
    public ApplicationUser? ApplicationUser { get; set; } // Navigation parameter
    [ForeignKey("game_id")]
    public int? GameId { get; set; } // Foreign key
    public Game? Game { get; set; } // Navigation parameter
    public string? ContactName { get; set; }
    public string? ContactInfo { get; set; }
    public bool Approuved { get; set; } = false; 
    public DateTime? CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
}

public class GameDemandEntityConfiguration : BaseEntityConfiguration<GameDemand>
{
    public override void Configure(EntityTypeBuilder<GameDemand> builder)
    {
        base.Configure(builder);
        builder.HasOne(gameDemand => gameDemand.ApplicationUser)
            .WithMany(applicationUser => applicationUser.GameDemands).HasForeignKey(gameDemand => gameDemand.UserId);
        builder.HasOne(gameDemand => gameDemand.Game)
            .WithMany(game => game.GameDemands).HasForeignKey(gameDemand => gameDemand.UserId);
    }
}
