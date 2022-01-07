namespace API.DbContext.Entity.Interface;

public interface IMustHaveTenant
{
    public string? Tenant { get; set; }
}