using API.Db.Entity.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Db.Entity;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IBaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
        builder.Property(p => p.CreatedOn).HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.LastModifiedOn).HasDefaultValueSql("GETDATE()");
    }
}