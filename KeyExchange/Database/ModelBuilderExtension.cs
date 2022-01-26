using KeyExchange.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeyExchange.Database;

public static class ModelBuilderExtensions
{
    public static void ApplyIdentityConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users", "IDENTITY");
        });
        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles", "IDENTITY");
        });
        builder.Entity<ApplicationRoleClaim>(entity =>
        {
            entity.ToTable("RoleClaims", "IDENTITY");
        });

        builder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("UserRoles", "IDENTITY");
        });

        builder.Entity<IdentityUserClaim<int>>(entity =>
        {
            entity.ToTable("UserClaims", "IDENTITY");
        });

        builder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("UserLogins", "IDENTITY");
        });
        builder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("UserTokens", "IDENTITY");
        });
    }
}
