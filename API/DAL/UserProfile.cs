using API.DbContext.Entity.Interface;

namespace API.Identity;

public class UserProfile : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public UserApplication User { get; set; }
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
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    public Guid? LastModifiedBy { get; set; } = Guid.Empty;
    public DateTime? LastModifiedOn { get; set; } = DateTime.Now;
}