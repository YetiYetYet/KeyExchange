using System.Security.Claims;
using API.Db;
using API.DTO.Authentification;
using API.Models;
using API.Service.Mailing;
using API.Service.User;
using API.Utils;
using API.Utils.Jwt;
using API.Utils.Validator;
using FluentValidation.Results;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Authentification;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbApplicationDbContext;
    private readonly IMailService _mailService;
    private readonly ICurrentUser _currentUser;
    
    public AuthController(IConfiguration configuration, ApplicationDbContext dbApplicationDbContext, IMailService mailService, ICurrentUser currentUser)
    {
        _dbApplicationDbContext = dbApplicationDbContext;
        _configuration = configuration;
        _mailService = mailService;
        _currentUser = currentUser;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        //Check if user exists
        if(_dbApplicationDbContext.Users.Any(x => x.Username == registerDto.Username))
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
        Role userRole = _dbApplicationDbContext.Roles.Single(x => x.Key == "User");
        
        // Create User and UserProfile
        Models.User? user = AutoMapperUtils.BasicAutoMapper<RegisterDto, Models.User>(registerDto);
        user.Role = userRole;
        user.CreatedBy = 0;
        user.LastModifiedBy = 0;
        _dbApplicationDbContext.Users.Add(user);
        await _dbApplicationDbContext.SaveChangesAsync();
        return Ok();
    }
    
    
    //  Just a test
    [HttpGet("get-my-name"), Authorize]
    public ActionResult<string> GetMyName()
    {
        Console.WriteLine(HttpContext.User.Identity?.Name);
        return Ok(HttpContext.User.Identity?.Name);
    }
    
    //  Just a test
    [HttpGet("get-my-id"), Authorize]
    public ActionResult<string> GetInfo()
    {
        return Ok(_currentUser.GetUserId());
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
    {
        if(!_dbApplicationDbContext.Users.Any(x => x.Username == loginDto.Username))
            return BadRequest("This userApplication doesn't exist");

        Models.User user = _dbApplicationDbContext.Users.Include(user => user.Role).First(x => x.Username == loginDto.Username);
        var isGoodPass = Argon2.Verify(user.Password, loginDto.Password);

        if (!isGoodPass)
        {
            user.AccessFailedCount++;
            await _dbApplicationDbContext.SaveChangesAsync();
            return BadRequest("Wrong password");
        }

        var token = JwtUtils.CreateToken(user, 
            _configuration.GetSection("SecuritySettings:JwtSettings:key").Value,
            int.Parse(_configuration.GetSection("SecuritySettings:JwtSettings:tokenExpirationInMinutes").Value));
        
        user.LastLogin = DateTime.Now;
        await _dbApplicationDbContext.SaveChangesAsync();
        
        return Ok($"bearer {token}");
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        Models.User? user = await _dbApplicationDbContext.Users.Where(user => user.Email == request.Email).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User with this email don't exist.");
        _mailService.SendTestAsync();
        return Ok();
    }
}