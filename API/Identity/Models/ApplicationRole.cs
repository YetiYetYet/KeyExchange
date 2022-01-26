using API.Db.Entity.Entity.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Models;

public class ApplicationRole : IdentityRole<int>, ISoftDelete
{
    public string Key { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ValidUpto { get; set; }
    public string? Description { get; set; }
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}