using API.Db;
using API.DTO;
using API.Models;
using API.Service;
using API.Utils;
using API.Utils.Validator;
using FluentValidation.Results;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ContextApi _dbContext;
    
    public AuthController(IConfiguration configuration, ContextApi dbContext)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        //Check if user exists
        if(_dbContext.ApplicationUsers.Any(x => x.Username == registerDto.Username))
            return BadRequest("This username already exists");
        
        RegisterUserRequestValidator validator = new();
        ValidationResult results = await validator.ValidateAsync(registerDto);

        if (!results.IsValid)
        {
            return BadRequest(results.ToString("~"));
        }
        
        // Hash Password
        registerDto.Password = Argon2.Hash(registerDto.Password);
        
        // Get Basic Roles
        Role userRole = _dbContext.Roles.Single(x => x.Key == "User");
        
        // Create User and UserProfile
        User? user = AutoMapperUtils.BasicAutoMapper<RegisterDto, User>(registerDto);
        user.Role = userRole;
        user.CreatedBy = Guid.Empty;
        user.LastModifiedBy = Guid.Empty;
        _dbContext.ApplicationUsers.Add(user);
        await _dbContext.SaveChangesAsync();
        //UserDto userDto = AutoMapperUtils.BasicAutoMapper<User, UserDto>(user);
        return Ok();
    }
    
    
    //  Just a test
    [HttpGet("test"), Authorize]
    public ActionResult<string> GetInfo()
    {
        Console.WriteLine(HttpContext.User.Identity.Name);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
    {
        if(!_dbContext.ApplicationUsers.Any(x => x.Username == loginDto.Username))
            return BadRequest("This userApplication doesn't exist");

        User user = _dbContext.ApplicationUsers.Include(user => user.Role).First(x => x.Username == loginDto.Username);
        var isGoodPass = Argon2.Verify(user.Password, loginDto.Password);

        if (!isGoodPass)
        {
            user.AccessFailedCount++;
            user.LastModifiedBy = Guid.Empty;
            user.LastModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return BadRequest("Wrong password");
        }

        var token = JwtUtils.CreateToken(user, _configuration.GetSection("secret").Value);
        
        user.LastLogin = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        
        return Ok($"bearer {token}");
    }
    
    
}