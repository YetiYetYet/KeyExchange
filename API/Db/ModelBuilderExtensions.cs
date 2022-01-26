using System.Linq.Expressions;
using API.Identity.Models;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

namespace API;

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

    public static ModelBuilder AppendGlobalQueryFilter<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
    {
        // gets a list of entities that implement the interface TInterface
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            var parameterType = Expression.Parameter(modelBuilder.Entity(entity).Metadata.ClrType);
            var expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

            // get existing query filters of the entity(s)
            var currentQueryFilter = modelBuilder.Entity(entity).GetQueryFilter();
            if (currentQueryFilter != null)
            {
                var currentExpressionFilter = ReplacingExpressionVisitor.Replace(currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);

                // Append new query filter with the existing filter
                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }

            var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);

            // applies the filter to the entity(s)
            modelBuilder.Entity(entity).HasQueryFilter(lambdaExpression);
        }

        return modelBuilder;
    }

    private static LambdaExpression? GetQueryFilter(this EntityTypeBuilder builder)
    {
        return builder?.Metadata?.GetQueryFilter();
    }
}