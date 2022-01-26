using System.Net;
using API.Exception;

namespace DN.WebApi.Application.Common.Exceptions;

public class EntityAlreadyExistsException : CustomExeption.CustomException
{
    public EntityAlreadyExistsException(string message)
    : base(message, null, HttpStatusCode.Conflict)
    {
    }
}