using System.Net;
using API.Exception;

namespace DN.WebApi.Application.Common.Exceptions;

public class EntityNotFoundException : CustomExeption.CustomException
{
    public EntityNotFoundException(string message)
    : base(message, null, HttpStatusCode.NotFound)
    {
    }
}