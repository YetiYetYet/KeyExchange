using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Db.Entity.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Models;

public class Game : IBaseEntity, ISoftDelete
{
    [Key]
    public int Id { get; set; }
    public int? CreatedBy { get; set; }
    [ForeignKey("user_id")]
    public int? UserId { get; set; } // Foreign key
    public User? User { get; set; } // reference navigation
    public DateTime? CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? Name { get; set; }
    public string? Platforme { get; set; }
    public bool GeneratedInfo { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Reviews { get; set; }
    public string? TumbnailUrl { get; set; }
    public string? Key { get; set; }
    public string? ObtenaidBy { get; set; }
    public string? PublicComment { get; set; }
    public string? OwnerComment { get; set; }
    public string? AdminComment { get; set; }
    public List<GameDemand>? GameDemands { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime? GivenDate { get; set; }
    public string? GivenTo { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
}

public class GameEntityConfiguration : Db.Entity.BaseEntityConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        base.Configure(builder);
        builder.HasOne(game => game.User)
            .WithMany(user => user.Games)
            .HasForeignKey(game => game.UserId);
    }
}