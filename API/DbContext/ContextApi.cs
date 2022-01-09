using API.Catalog;
using API.DAL;
using API.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.DbContext;

public class ContextApi : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<UserApplication> Users { get; set; }
    public DbSet<GameDemand> GameDemands { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ContextApi(IConfiguration configuration, DbContextOptions<ContextApi> options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("key_exchange");

        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<UserApplication>());
        modelBuilder.Entity<UserApplication>().HasIndex(p => p.Username).IsUnique();

        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<Role>());
        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<UserProfile>());
        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<GameDemand>());
        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<Game>());
        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<RoleClaim>());

        base.OnModelCreating(modelBuilder);
    }
}