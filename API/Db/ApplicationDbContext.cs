using API.Db.Entity;
using API.Db.Entity.Entity.Interface;
using API.Models;
using API.Service.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Db;

public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    private readonly IConfiguration _configuration;
    
    public DbSet<GameDemand> GameDemands { get; set; }
    public DbSet<Game> Games { get; set; }
    
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
        modelBuilder.HasDefaultSchema("keys_exchange_test_identity");

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RoleGameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameDemandEntityConfiguration());

        base.OnModelCreating(modelBuilder);
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
                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }
        var results = await base.SaveChangesAsync(cancellationToken);
        return results;
    }
}