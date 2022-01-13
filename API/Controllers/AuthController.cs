using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Db;
using API.DTO;
using API.Models;
using API.Service;
using API.Utils;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private ContextApi _dbContext;
    private readonly IUserService _userService;
    public AuthController(IConfiguration configuration, ContextApi dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        //Check if user exists
        if(_dbContext.ApplicationUsers.Any(x => x.Username == registerDto.Username))
            return BadRequest("This username already exists");
        
        // Hash Password
        registerDto.Password = Argon2.Hash(registerDto.Password);
        
        // Get Basic Roles
        Role userRole = _dbContext.Roles.Single(x => x.Key == "User");
        
        // Create User and UserProfile
        ApplicationUser? user = AutoMapperUtils.BasicAutoMapper<RegisterDto, ApplicationUser>(registerDto);
        user.Role = userRole;
        user.UserProfile = new UserProfile();
        _dbContext.ApplicationUsers.Add(user);
        await _dbContext.SaveChangesAsync();
        return Ok(user);
    }
    
    //  Just a test
    [HttpGet, Authorize]
    public ActionResult<string> GetMe()
    {
        var userName = _userService.GetMyName();
        return Ok(userName);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
    {
        if(!_dbContext.ApplicationUsers.Any(x => x.Username == loginDto.Username))
            return BadRequest("This userApplication doesn't exist");

        ApplicationUser user = _dbContext.ApplicationUsers.Include(user => user.Role).First(x => x.Username == loginDto.Username);
        var goodPass = Argon2.Verify(user.Password, loginDto.Password);

        if (!goodPass)
        {
            user.AccessFailedCount++;
            await _dbContext.SaveChangesAsync();
            return BadRequest("Wrong password");
        }

        var token = CreateToken(user);
        
        user.LastLogin = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return Ok($"bearer {token}");
    }
    
    private string CreateToken(ApplicationUser user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.Key),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("secret").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}