using Microsoft.EntityFrameworkCore;

namespace API;

public class DbContextApi : DbContext
{
    private IConfiguration _configuration;
    
    public DbContextApi(IConfiguration configuration, DbContextOptions<DbContextApi> options)
    {
        _configuration = configuration;
    }
}