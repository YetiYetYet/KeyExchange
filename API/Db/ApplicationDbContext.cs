using System.Data;
using API.Db.Entity;
using API.Db.Entity.Entity.Interface;
using API.Identity.Models;
using API.Models;
using API.Service.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Db;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, ApplicationRoleClaim, IdentityUserToken<int>>
{
    private readonly IConfiguration _configuration;
    public IDbConnection Connection => Database.GetDbConnection();

    public DbSet<GameDemand> GameDemands => Set<GameDemand>();
    public DbSet<Game> Games  => Set<Game>();
    
    private readonly ICurrentUser _currentUserService;

    public ApplicationDbContext(IConfiguration configuration, DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUserService)
    {
        _configuration = configuration;
        _currentUserService = currentUserService;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyIdentityConfiguration();
        modelBuilder.HasDefaultSchema("keys_exchange_entity");
        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameDemandEntityConfiguration());

        
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int currentUserId = _currentUserService.GetUserId();
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