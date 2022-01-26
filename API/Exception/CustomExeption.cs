using System.Net;

namespace API.Exception;

public class CustomExeption
{
    public class CustomException : System.Exception
    {
        public List<string>? ErrorMessages { get; }
    
        public HttpStatusCode StatusCode { get; }
    
        public CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
        }
    }
}