using API.DbContext;
using API.DTO;
using API.Identity;
using API.Utils;
using AutoMapper;
using Isopoh.Cryptography.Argon2;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private ContextApi _dbContext;
    public AuthController(IConfiguration configuration, ILogger<AuthController> logger, ContextApi dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        //Check if user exists
        if(_dbContext.Users.Any(x => x.Username == registerDto.Username))
            return BadRequest("This username already exists");
        
        // Hash Password
        registerDto.Password = Argon2.Hash(registerDto.Password);
        
        // Get Basic Roles
        Role userRole = _dbContext.Roles.Single(x => x.Key == "User");
        
        // Create User and UserProfile
        UserApplication? user = AutoMapperUtils.BasicAutoMapper<RegisterDto, UserApplication>(registerDto);
        user.Role = userRole;
        _dbContext.Users.Add(user);
        UserProfile userProfile = new() { User = user, };
        _dbContext.UserProfiles.Add(userProfile);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
    {
        if(!_dbContext.Users.Any(x => x.Username == loginDto.Username))
            return BadRequest("This userApplication doesn't exist");

        UserApplication userApplication = _dbContext.Users.First(x => x.Username == loginDto.Username);
        var goodPass = Argon2.Verify(userApplication.Password, loginDto.Password);

        if (!goodPass)
        {
            userApplication.AccessFailedCount++;
            await _dbContext.SaveChangesAsync();
            return BadRequest("Wrong password");
        }
            

        var tokenExpiration = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
        var token = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
            .WithSecret(_configuration.GetValue<string>("secret"))
            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            .AddClaim("username", userApplication.Username)
            .AddClaim("role", userApplication.Role)
            .Encode();
        
        userApplication.LastLogin = DateTime.Now;
        userApplication.RefreshToken = token;
        userApplication.RefreshTokenExpiration = DateTime.Now.AddHours(1);

        await _dbContext.SaveChangesAsync();
        return Ok(token);
    }
}