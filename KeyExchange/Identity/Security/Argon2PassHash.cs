using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;

namespace KeyExchange.Identity.Security;

public class Argon2PassHash<TUser> : IPasswordHasher<TUser> where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        var hashedPassword = Argon2.Hash(password);
        return hashedPassword;
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        var isValid = Argon2.Verify(hashedPassword, providedPassword);
        return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}