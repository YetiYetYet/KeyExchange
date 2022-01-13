using API.Db.Entity.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Models;

public class User : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Discord { get; set; }
    public string? PhoneNumber { get; set; }
    public Role Role { get; set; }
    public bool IsPublic { get; set; } = false;
    public bool ShowPhoneNumber { get; set; } = false;
    public bool ShowEmail { get; set; } = false;
    public bool ShowDiscord { get; set; } = false;
    public bool ShowFirstName { get; set; } = false;
    public bool ShowLastName { get; set; } = false;
    public string? PictureUri { get; set; } =
        "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/Breezeicons-actions-22-im-user.svg/1200px-Breezeicons-actions-22-im-user.svg.png";
    public string? Description { get; set; }
    public string? OtherLink { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Game>? Games { get; set; }
    public List<GameDemand>? GameDemands { get; set; }
    public DateTime? LastLogin { get; set; }
    public int AccessFailedCount { get; set; } = 0;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    public Guid? LastModifiedBy { get; set; } = Guid.Empty;
    public DateTime? LastModifiedOn { get; set; } = DateTime.Now;
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}

public class UserEntityConfiguration : Db.Entity.BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.HasIndex(p => p.Username).IsUnique();
        builder.Navigation(r => r.Role).AutoInclude();
    }
}