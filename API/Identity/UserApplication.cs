﻿using System.ComponentModel.DataAnnotations.Schema;
using API.DbContext.Entity.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Identity;

public class UserApplication : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Discord { get; set; }
    public string? PhoneNumber { get; set; }
    public Role? Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLogin { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public int AccessFailedCount { get; set; } = 0;
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public DateTime? CreatedOn { get; set; } = DateTime.Now;
    public Guid? LastModifiedBy { get; set; } = Guid.Empty;
    public DateTime? LastModifiedOn { get; set; } = DateTime.Now;
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
}