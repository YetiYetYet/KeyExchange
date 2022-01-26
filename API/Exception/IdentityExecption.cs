using System.Net;

namespace API.Exception;

public class IdentityExecption
{
    public class IdentityException : CustomExeption.CustomException
    {
        public IdentityException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, errors, statusCode)
        {
        }
    }
}