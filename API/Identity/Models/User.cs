﻿using API.Db.Entity.Entity.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Models;

public class User : IdentityUser<int>, ISoftDelete
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Discord { get; set; }
    public Role Role { get; set; }
    public bool IsPublic { get; set; } = true;
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
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool SoftDeleted { get; set; } = false;
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
}

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // builder.HasIndex(p => p.Username).IsUnique();
        // builder.HasIndex(p => p.Email).IsUnique();
        // builder.Navigation(r => r.Role).AutoInclude();
        // builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
        builder.Property(p => p.CreatedOn).HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.LastModifiedOn).HasDefaultValueSql("GETDATE()");
    }
}