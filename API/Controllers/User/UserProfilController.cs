﻿using API.Db;
using API.DTO.Game;
using API.DTO.Users;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.User;

[ApiController]
[Route("[controller]")]
public class UserProfilController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ContextApi _context;

    
    public UserProfilController(IConfiguration configuration, ContextApi dbContext)
    {
        _context = dbContext;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllGames()
    {
        var user = await _context.ApplicationUsers.Where(user => user.IsPublic && !user.SoftDeleted).ToListAsync();
        var userProfileDtos = AutoMapperUtils.TupleAutoMapper<Models.User, UserProfileDto>(user, new List<(Type SourceType, Type Destination)>() {(typeof(Role), typeof(RoleDto))});
        var hidedUserProfil = userProfileDtos.Select(HideInfo).ToList();
        return Ok(hidedUserProfil);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserProfileDto>> GetUserByGuid(Guid id)
    {
        var user = await _context.ApplicationUsers.Where(user => user.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound("This user didn't exist");
        }
        if (!user.IsPublic || user.SoftDeleted)
        {
            return Forbid("Forbidden");
        }
        UserProfileDto userProfileDto = AutoMapperUtils.TupleAutoMapper<Models.User, UserProfileDto>(user, new List<(Type SourceType, Type Destination)>() {(typeof(Role), typeof(RoleDto))});
        UserProfileDto hidedUserProfil = HideInfo(userProfileDto);
        return hidedUserProfil;
    }

    private UserProfileDto HideInfo(UserProfileDto userProfileDto)
    {
        UserProfileDto hidedUserProfil = userProfileDto.ShallowCopy();
        if (!userProfileDto.ShowDiscord)
        {
            hidedUserProfil.Discord = null;
        }
        if (!userProfileDto.ShowEmail)
        {
            hidedUserProfil.Email = null;
        }
        if (!userProfileDto.ShowFirstName)
        {
            hidedUserProfil.FirstName = null;
        }
        if (!userProfileDto.ShowLastName)
        {
            hidedUserProfil.LastName = null;
        }
        if (!userProfileDto.ShowPhoneNumber)
        {
            hidedUserProfil.PhoneNumber = null;
        }
        return hidedUserProfil;
    }
    
    
}