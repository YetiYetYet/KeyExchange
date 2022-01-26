using KeyExchange.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KeyExchange.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<int>, ISoftDelete
{
    public string? Description { get; set; }
    public string? Group { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
    
    public ApplicationRoleClaim()
    {
    }
    
    public ApplicationRoleClaim(string? roleClaimDescription = null, string? roleClaimGroup = null)
    {
        Description = roleClaimDescription;
        Group = roleClaimGroup;
    }

    
}