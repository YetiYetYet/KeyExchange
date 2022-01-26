using KeyExchange.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyExchange.Database;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, ISoftDelete, IBaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(p => p.CreatedOn).HasDefaultValueSql("now()");
    }
}