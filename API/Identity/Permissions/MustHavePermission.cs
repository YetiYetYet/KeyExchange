using Microsoft.AspNetCore.Authorization;

namespace API.Identity;

public class MustHavePermission : AuthorizeAttribute
{
    public MustHavePermission(string permission)
    {
        Policy = permission;
    }
}