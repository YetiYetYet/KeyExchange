using API.Db.Entity;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Db;

public class ContextApi : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<User> ApplicationUsers { get; set; }
    public DbSet<GameDemand> GameDemands { get; set; }
    public DbSet<Game> Games { get; set; }
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
        modelBuilder.HasDefaultSchema("keys_exchange");

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BaseEntityConfiguration<Role>());
        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameDemandEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}