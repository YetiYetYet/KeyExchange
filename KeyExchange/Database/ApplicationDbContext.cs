using KeyExchange.Database.Interfaces;
using KeyExchange.Identity;
using KeyExchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeyExchange.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, ApplicationRoleClaim, IdentityUserToken<int>>
{
    private readonly IConfiguration _configuration;
    public DbSet<GameDemand> GameDemands => Set<GameDemand>();
    public DbSet<Game> Games  => Set<Game>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyIdentityConfiguration();
        modelBuilder.HasDefaultSchema("keys_exchange");
        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameDemandEntityConfiguration());
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int currentUserId = 1234;//_currentUserService.GetUserId();
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.LastModifiedBy = currentUserId;
                    entry.Entity.LastModifiedOn = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.Now;
                    entry.Entity.LastModifiedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = currentUserId;
                        softDelete.DeletedOn = DateTime.Now;
                        softDelete.SoftDeleted = true;
                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }
        var results = await base.SaveChangesAsync(cancellationToken);
        return results;
    }
}