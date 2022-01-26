using KeyExchange.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KeyExchange.Identity;

public class ApplicationRole : IdentityRole<int>, ISoftDelete
{
    public string? Key { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ValidUpto { get; set; }
    public string? Description { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

}